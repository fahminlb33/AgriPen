import httpClient from "@/services/helpers/http_client";

import { DiseaseItem, LandItem, WeatherItem } from "./maps.types";

export async function weather(): Promise<WeatherItem[] | null> {
	try {
		const response = await httpClient.get("/maps/weather");
		return response.data.map((item: any) => ({
			...item,
			timestamp: new Date(item.timestamp),
		}));
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function disease(): Promise<DiseaseItem[] | null> {
	try {
		const response = await httpClient.get("/maps/disease");
		return response.data.map((item: any) => ({
			...item,
			timestamp: new Date(item.timestamp),
		}));
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function land(): Promise<LandItem[] | null> {
	try {
		const response = await httpClient.get("/maps/land-observation");
		return response.data.map((item: any) => ({
			...item,
			timestamp: new Date(item.timestamp),
		}));
	} catch (err) {
		console.error(err);
		return null;
	}
}
