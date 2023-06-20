export type SearchParams = {
  season_id: string|null;
  temperature: number | "";
  humidity: number | "";
  soil_moisture: number |"";
}

export type ListItem = {
  id:  string;
  name: string;
  nameID: string;
  season: string;
}

export type SeasonItem = {
  label:  string;
  value: string;
}

export type Recommendation = {
  id:        string;
  name:      string;
  nameID:    string;
  season:    Season;
  nitrogen:  Nitrogen[];
  phosporus: PhosporusPotassium[];
  potassium: PhosporusPotassium[];
  ph:        Ph[];
}

export type Nitrogen = {
  nitrogen: number;
  notes:    string;
}

export type Ph = {
  optimal: number;
  minimum: number;
  notes:   string;
}

export type PhosporusPotassium = {
  category1: number;
  category2: number;
  category3: number;
  category4: number;
  category5: number;
  notes:     string;
}

export type Season = {
  season:           string;
  tempDayLow:       number;
  tempDayHigh:      number;
  tempNightLow:     number;
  tempNightHigh:    number;
  humidityLow:      number;
  humidityHigh:     number;
  soilMoistureLow:  number;
  soilMoistureHigh: number;
}
