# Variables
MODEL_URL="https://blob.kodesiana.com/kodesiana-ai-public/models/skripsi_klasifikasi_penyakit_padi/python/model-289106b3e245440c97dedef10cafd8c1l.h5"
CLASS_NAMES_URL="https://blob.kodesiana.com/kodesiana-ai-public/models/skripsi_klasifikasi_penyakit_padi/python/class_names-289106b3e245440c97dedef10cafd8c1l.z"

CONTAINER_NAME="agripen-worker"
IMAGE_FULLNAME="agripen.azurecr.io/agripen-worker:v2.0"

# Commands
download:
	mkdir -p data
	curl -o ${CURDIR}/data/model.h5 ${MODEL_URL}
	curl -o ${CURDIR}/data/class_names.z ${CLASS_NAMES_URL}
	echo "Model and class names downloaded"

run:
	uvicorn app:app --port 8080

debug:
	uvicorn app:app --port 8080 --reload

build:
	docker build --build-arg MODEL_URL="${MODEL_URL}" --build-arg CLASS_NAMES_URL="${CLASS_NAMES_URL}" -t ${IMAGE_FULLNAME} .
	docker push ${IMAGE_FULLNAME}

# Directives
.PHONY: download run debug build
