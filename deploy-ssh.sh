FRONTEND_TAG=v1.5
BACKEND_TAG=v1.14
WORKER_TAG=v1.3
NGINX_TAG=v1.5

docker stop $(docker ps -a | grep -v -i "redis" | awk 'NR>1 {print $1}')
docker rm $(docker ps -a | grep -v -i "redis" | awk 'NR>1 {print $1}')

# docker run -d --name redis --network agripen redis || true
docker run \
  -d \
  --name agripen-frontend \
  --network agripen \
  --restart always \
  agripen.azurecr.io/agripen-frontend:$FRONTEND_TAG || true

docker run \
  -d \
  -v ./backend-env.json:/app/appsettings.json:ro \
  --env ASPNETCORE_ENVIRONMENT=Production \
  --name agripen-backend \
  --network agripen \
  --restart always \
  agripen.azurecr.io/agripen:$BACKEND_TAG || true

docker run \
  -d \
  --env-file ./worker.env \
  --name agripen-worker \
  --network agripen \
  --restart always \
  agripen.azurecr.io/agripen-worker:$WORKER_TAG || true

docker run \
  -d \
  -p 80:80 \
  -p 443:443 \
  --name agripen-nginx \
  --network agripen \
  --restart always \
  agripen.azurecr.io/agripen-nginx:$NGINX_TAG || true
