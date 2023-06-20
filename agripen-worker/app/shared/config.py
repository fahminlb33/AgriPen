import os

class AgriConfig:
    FORECAST_CSV_URL: str
    KECAMATAN_CSV_URL: str

    AZURE_STORAGE_CONNECTION_STRING: str
    AZURE_STORAGE_CONTAINER_NAME: str = "disease-prediction"

    AZURE_SQL_CONNECTION_STRING: str

    MODEL_PATH: str = "models/model.h5"
    CLASS_NAMES_PATH: str = "models/class_names.z"

    REDIS_HOST: str = "localhost"
    REDIS_PORT: int = 6379
    REDIS_TOPIC: str = "analyze_image"

    class Config:
        env_file = ".env"


config = AgriConfig()
