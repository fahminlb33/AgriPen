#!/bin/bash

FRONTEND_TAG=v1.6
BACKEND_TAG=v1.14
WORKER_TAG=v1.4
NGINX_TAG=v1.6

REPOSITORY=agripen.azurecr.io

pushd agripen-backend
docker build -t $REPOSITORY/agripen-backend:latest -f AgriPen/Dockerfile .
docker push $REPOSITORY/agripen:$BACKEND_TAG
popd

pushd agripen-frontend
source build-and-push.sh
popd

pushd agripen-worker
make build
popd

pushd agripen-nginx
make build
popd
