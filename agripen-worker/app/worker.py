from dotenv import load_dotenv
load_dotenv()

import os
import logging
import tempfile

import signal
import redis

from sqlalchemy import create_engine, text
from azure.storage.blob import BlobServiceClient, BlobClient, ContainerClient

from shared.config import config
from shared.helpers import safe_delete, MAP_RESULT, MAP_STATUS
from shared.predictor import PredictorService

def signal_handler(signal, frame):
    global interrupted
    interrupted = True

def analyze_image(id: str, predictor_service: PredictorService, container_client: ContainerClient, engine):
    # --- 1 - DOWNLOAD FROM BLOB STORAGE

    # load image from blob storage
    local_filepath = os.path.join(tempfile.gettempdir(), f"{id}.jpg")
    with open(local_filepath, "wb") as f:
        original_blob_name = f"predictions/{id}/unknown.jpg"
        blob_client: BlobClient = container_client.get_blob_client(original_blob_name)
        download_stream = blob_client.download_blob()
        f.write(download_stream.readall())


    # --- 2 - INFERENCE
    # get file size
    file_size = os.path.getsize(local_filepath)
    logging.info(f"Uploaded file size: {file_size}")

    # resize image to maximum of MAX_HEIGHT
    logging.info(f"Constraining uploaded image size...")
    resized_path = predictor_service.constrain_image_size(local_filepath)

    # make prediction
    logging.info(f"Running prediction...")
    (prediction_proba, heatmap_path, superimposed_path, masked_path, severity) \
        = predictor_service.predict(resized_path, tempfile.gettempdir())
    

    # --- 3 - UPDATE DATABASE

    # update database
    with engine.connect() as conn:
      # update prediction
      predicted_class = predictor_service.get_most_likely_class(prediction_proba)
      result = MAP_RESULT[predicted_class]
      status = MAP_STATUS["SUCCESS"]

      sql = text(f"UPDATE disease_predictions SET severity = '{severity}', result = {result}, status = {status} WHERE id = '{id}'")
      conn.execute(sql)

      # update probabilities
      class_probabilities = {
        predictor_service.get_class_from_prediction(i): round(v, 4) for i, v in enumerate(prediction_proba.tolist())
      }

      proba_healthy = class_probabilities["HEALTHY"]
      proba_bacterial_blight = class_probabilities["BACTERIALBLIGHT"]
      proba_blast = class_probabilities["BLAST"]
      proba_brownspot = class_probabilities["BROWNSPOT"]
      proba_tungro = class_probabilities["TUNGRO"]

      sql = text(f"UPDATE disease_probability SET bacterial_blight = {proba_bacterial_blight}, blast = {proba_blast}, brown_spot = {proba_brownspot}, tungro = {proba_tungro}, healthy = {proba_healthy} WHERE prediction_id = '{id}'")
      conn.execute(sql)

      # commit
      conn.commit()
    

    # --- 4 - UPLOAD TO BLOB STORAGE
    logging.info(f"Uploading results to blob storage...")

    # upload queue
    upload_queue = [
        ("masked", masked_path, f"predictions/{id}/masked.jpg"),
        ("heatmap", heatmap_path, f"predictions/{id}/heatmap.jpg"),
        ("superimposed", superimposed_path, f"predictions/{id}/superimposed.jpg"),
    ]

    # process all files
    for (key, path, blob_name) in upload_queue:
        # get blob client
        heatmap_blob_client = container_client.get_blob_client(blob_name)

        # open read stream
        with open(path, "rb") as data:
            # create tags
            tags = {"InferenceID": id}

            # upload
            heatmap_blob_client.upload_blob(data, overwrite=True, tags=tags)

        # delete temp file
        safe_delete(path)


    # --- 5 - CLEAN UP

    # delete original file
    safe_delete(local_filepath)

    # delete resized file
    safe_delete(resized_path)


if __name__ == '__main__':
  # setup logging
  logging.basicConfig(level=logging.DEBUG, format='%(asctime)s %(levelname)s %(message)s')

  # register signal handler
  interrupted = False
  signal.signal(signal.SIGINT, signal_handler)
  signal.signal(signal.SIGTERM, signal_handler)

  # connect to db
  engine = create_engine(config.azure_sql_connection_string, echo=True)

  # create client
  blob_client: BlobServiceClient = BlobServiceClient.from_connection_string(config.azure_storage_connection_string)
  container_client: ContainerClient = blob_client.get_container_client(config.azure_storage_container_name)

  # load model
  predictor_service = PredictorService()
  predictor_service.load_model(config.model_path, config.class_names_path)

  # connect to redis
  client = redis.Redis(host=config.redis_host, port=config.redis_port)
  pubsub = client.pubsub()
  pubsub.subscribe(config.redis_topic)

  # listen for messages
  logging.info('Listening for messages...')
  while True and not interrupted:
    # poll for messages
    msg = pubsub.get_message(timeout=5)
    if msg is not None and msg['data'] != 1:
      id = msg['data'].decode('utf-8')
      logging.info(f'Processing message: {id}')

      analyze_image(id, predictor_service, container_client, engine)
