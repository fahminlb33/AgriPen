import { GpsAddress, LocalAddress, PaginationMeta } from "./shared";

export type UploadResponse = {
	id: string;
};

export type ListItem = {
	id: string;

	status: string;
	result: string;
	severity: number;
	probability: number;
	address: string;

	createdAt: Date;
};

export type ListResponse = {
	meta: PaginationMeta;
	data: ListItem[];
};

export type Probability = {
	bacterialBlight: number;
	blast: number;
	brownSpot: number;
	tungro: number;
	healthy: number;
};

export type Prediction = {
	id: string;
	severity: number;
	result: string;
	status: string;
	probabilities: Probability;
	images: string[];
	gpsAddress: GpsAddress;
	localAddress: LocalAddress;
	createdAt: Date;
	updatedAt: Date;
};
