import { GpsAddress, LocalAddress, PaginationMeta } from "./shared";

export type ListItem = {
	id: string;

	airTemperature: number;
	airHumidity: number;
	airHeatIndex: number;
	soilMoisture: number;
	sunIllumination: number;
	gpsLocation: string;

	createdAt: Date;
};

export type ListResponse = {
	meta: PaginationMeta;
	data: ListItem[];
};

export type Telemetry = {
	timestamp: Date;
	airTemperature: number;
	airHumidity: number;
	airHeatIndex: number;
	soilMoisture: number;
	sunIllumination: number;
};

export type Observation = {
	id: string;
	images: string[];
	telemetry: Telemetry[];
	gpsAddress: GpsAddress;
	localAddress: LocalAddress;
	createdAt: Date;
};
