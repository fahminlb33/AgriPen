#!/bin/bash

FRONTEND_TAG=v2.0
BACKEND_TAG=v2.0
WORKER_TAG=v2.0
NGINX_TAG=v2.0

REPOSITORY=agripen.azurecr.io

MODEL_URL="https://blob.kodesiana.com/kodesiana-ai-public/models/skripsi_klasifikasi_penyakit_padi/python/model-289106b3e245440c97dedef10cafd8c1l.h5"
CLASS_NAMES_URL="https://blob.kodesiana.com/kodesiana-ai-public/models/skripsi_klasifikasi_penyakit_padi/python/class_names-289106b3e245440c97dedef10cafd8c1l.z"

pushd agripen-backend
docker build -t $REPOSITORY/agripen:$BACKEND_TAG -f AgriPen/Dockerfile .
docker push $REPOSITORY/agripen:$BACKEND_TAG
popd

pushd agripen-frontend
docker build -t $REPOSITORY/agripen-frontend:$FRONTEND_TAG .
docker push $REPOSITORY/agripen-frontend:$FRONTEND_TAG
popd

pushd agripen-worker
docker build --build-arg MODEL_URL="${MODEL_URL}" --build-arg CLASS_NAMES_URL="${CLASS_NAMES_URL}" -t $REPOSITORY/agripen-worker:$WORKER_TAG .
docker push $REPOSITORY/agripen-worker:$WORKER_TAG
popd

pushd agripen-nginx
docker build -t $REPOSITORY/agripen-nginx:$NGINX_TAG .
docker push $REPOSITORY/agripen-nginx:$NGINX_TAG
popd
