import ulid

import pandas as pd
from sqlalchemy import create_engine, text

from shared.config import config
from shared.helpers import MAP_WEATHER, MAP_WIND

# create engine
engine = create_engine(config.azure_sql_connection_string, echo=True)

# load kecamatan
with engine.connect() as con:
    sql = text('SELECT id, code FROM local_addresses')
    kec_df = pd.DataFrame(con.execute(sql).fetchall(), columns=['id', 'code'])
    kec_df['code'] = kec_df['code'].astype(str)

# load forecast
columns = ['code', 'timestamp', 'temperature_low', 'temperature_high', 'humidity_low', 'humidity_high', 'humidity', 'temperature', 'weather', 'wind', 'wind_speed']
df = pd.read_csv('kecamatanforecast-jawabarat.csv', names=columns, delimiter=';')

# preprocess
df = df.dropna()
df['code'] = df['code'].astype(str)
df['id'] = df.apply(lambda _: ulid.new().str, axis=1)
df['timestamp'] = pd.to_datetime(df['timestamp'], format='%Y-%m-%d %H:%M:%S')
df['weather'] = df['weather'].map(MAP_WEATHER)
df['wind'] = df['wind'].map(MAP_WIND)

# merge kecamatan and forecast
df_clean = df.merge(kec_df.rename(columns={"id": "local_address_id"}), on='code', how='left')
df_clean = df_clean.drop(columns=['code'])

with engine.connect() as con:
    df_clean.to_sql('weather_predictions', con=con, if_exists='append', index=False)
