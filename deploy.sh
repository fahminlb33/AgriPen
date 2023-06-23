#!/bin/bash

FRONTEND_TAG=v2.1
BACKEND_TAG=v2.0
WORKER_TAG=v2.0
NGINX_TAG=v2.0

REPOSITORY=agripen.azurecr.io

docker stop $(docker ps -a | grep -v -i "redis" | awk 'NR>1 {print $1}')
docker rm $(docker ps -a | grep -v -i "redis" | awk 'NR>1 {print $1}')

docker image prune -f

# docker run -d --name redis --network agripen redis || true
docker run \
  -d \
  --name agripen-frontend \
  --network agripen \
  --restart always \
  $REPOSITORY/agripen-frontend:$FRONTEND_TAG || true

docker run \
  -d \
  -v $(pwd)/backend-env.json:/app/appsettings.json:ro \
  --env ASPNETCORE_ENVIRONMENT=Production \
  --name agripen-backend \
  --network agripen \
  --restart always \
  $REPOSITORY/agripen:$BACKEND_TAG || true

docker run \
  -d \
  --env-file ./worker.env \
  --name agripen-worker \
  --network agripen \
  --restart always \
  $REPOSITORY/agripen-worker:$WORKER_TAG || true

docker run \
  -d \
  -p 80:80 \
  -p 443:443 \
  --name agripen-nginx \
  --network agripen \
  --restart always \
  $REPOSITORY/agripen-nginx:$NGINX_TAG || true
