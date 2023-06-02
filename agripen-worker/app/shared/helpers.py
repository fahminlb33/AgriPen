import os
import logging


def safe_delete(path: str):
  try:
    os.remove(path)
  except Exception as e:
    logging.error("Error deleting file: %s", e)
    logging.error(e)


MAP_STATUS = {
  "QUEUED": 0,
  "PROCESSING": 1,
  "SUCCESS": 2,
  "FAILED": 3,
}

MAP_RESULT = {
  "HEALTHY": 1,
  "BACTERIALBLIGHT": 2,
  "BLAST": 3,
  "BROWNSPOT": 4,
  "TUNGRO": 5,
}

MAP_WEATHER = {
    0: 0,
    100: 0,

    1: 1,
    101: 1,
    2: 1,
    102: 1,
    
    3: 2,
    103: 2,
    
    4: 3,
    104: 3,

    5: 4,

    10: 5,

    45: 6,

    60: 7,

    61: 8,

    63: 9,

    80: 10,

    95: 11,

    97: 12,
}

MAP_WIND = {
    "N": 0,
    "NE": 1,
    "E": 2,
    "SE": 3,
    "S": 4,
    "SW": 5,
    "W": 6,
    "NW": 7,
}