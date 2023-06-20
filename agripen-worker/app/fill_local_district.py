import ulid

import pandas as pd
from sqlalchemy import create_engine

from shared.config import config

# create engine
engine = create_engine(config.azure_sql_connection_string, echo=True)

# load kecamatan
columns = ['code', 'kecamatan', 'kabupaten', 'provinsi', 'latitude', 'longitude']
df = pd.read_csv(config.KECAMATAN_CSV_URL, names=columns, delimiter=';')
df['id'] = df.apply(lambda _: ulid.new().str, axis=1)

with engine.connect() as con:
  df.to_sql('local_addresses', con=engine, if_exists='append', index=False)
