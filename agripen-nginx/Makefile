# Variables
IMAGE_FULLNAME="agripen.azurecr.io/agripen-nginx:v1.6"

# Commands
build:
	docker build -t ${IMAGE_FULLNAME} .
	docker push ${IMAGE_FULLNAME}

# Directives
.PHONY: build
