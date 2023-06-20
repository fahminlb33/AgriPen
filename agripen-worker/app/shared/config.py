from pydantic import BaseSettings

class AgriConfig(BaseSettings):
    FORECAST_CSV_URL: str
    KECAMATAN_CSV_URL: str

    AZURE_STORAGE_CONNECTION_STRING: str
    AZURE_STORAGE_CONTAINER_NAME: str = "disease-prediction"

    AZURE_SQL_CONNECTION_STRING: str

    MODEL_PATH: str = "data/model.h5"
    CLASS_NAMES_PATH: str = "data/class_names.z"

    REDIS_HOST: str = "localhost"
    REDIS_PORT: int = 6379
    REDIS_TOPIC: str = "analyze_image"

    class Config:
        env_file = ".env"


config = AgriConfig()
