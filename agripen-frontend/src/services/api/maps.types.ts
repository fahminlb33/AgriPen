export type WeatherItem = {
	id: string;
	latitude: number;
	longitude: number;
	kecamatan: string;
	timestamp: Date;

	temperatureLow: number;
	temperatureHigh: number;
	humidityLow: number;
	humidityHigh: number;
	humidity: number;
	weather: string;
	wind: string;
	windSpeed: number;
};

export type DiseaseItem = {
	id: string;
	latitude: number;
	longitude: number;
	timestamp: Date;
	address: string;

	severity: number;
	probability: number;
	result: string;
	status: string;
};

export type LandItem = {
	id: string;
	latitude: number;
	longitude: number;
	timestamp: Date;
	address: string;

	airTemperature: number;
	airHumidity: number;
	airHeatIndex: number;
	soilMoisture: number;
	sunIllumination: number;
};
