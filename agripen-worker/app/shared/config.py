import os

class AgriConfig:
    forecast_csv_url = os.getenv("FORECAST_CSV_URL")
    kecamatan_csv_url = os.getenv("KECAMATAN_CSV_URL")

    azure_storage_connection_string = os.getenv("AZURE_STORAGE_CONNECTION_STRING")
    azure_storage_container_name = os.getenv("AZURE_STORAGE_CONTAINER_NAME", "disease-prediction")

    azure_sql_connection_string = os.getenv("AZURE_SQL_CONNECTION_STRING")

    model_path = os.getenv("MODEL_PATH", "models/model.h5")
    class_names_path = os.getenv("CLASS_NAMES_PATH", "models/class_names.z")

    redis_host = os.getenv("REDIS_HOST", "localhost")
    redis_port = int(os.getenv("REDIS_PORT", 6379))
    redis_topic = os.getenv("REDIS_TOPIC", "analyze_image")


config = AgriConfig()
