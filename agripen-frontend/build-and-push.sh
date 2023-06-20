#!/bin/bash
PACKAGE_VERSION=$(node -p -e "require('./package.json').version")
IMAGE_TAG="agripen.azurecr.io/agripen-frontend:v${PACKAGE_VERSION}"

docker build -t ${IMAGE_TAG} .
docker push ${IMAGE_TAG}
