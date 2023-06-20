import httpClient from "@/services/helpers/http_client";

import { Recommendation, ListItem, SearchParams, SeasonItem } from "./recommendation.types";

export async function listSeasons(): Promise<SeasonItem[] | null> {
	try {
		const response = await httpClient.get("/recommendation/seasons");
		return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function getPlant(id: string): Promise<Recommendation | null> {
	try {
		const response = await httpClient.get(`/recommendation/plants/${id}`);
		return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function findByPlant(
	name: string,
): Promise<ListItem[] | null> {
	try {
		const response = await httpClient.get("/recommendation/plants", {
			params: { q: name },
		});
		return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function findByObservations(
	id: string,
): Promise<ListItem[] | null> {
	try {
		const response = await httpClient.get(`/recommendation/observations/${id}`);
		return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function findByParams(
	params: SearchParams,
): Promise<ListItem[] | null> {
	try {
		const response = await httpClient.get("/recommendation/params", {
			params: {
				season_id: params.season_id,
				t: params.temperature,
				rh: params.humidity,
				srh: params.soil_moisture,
			},
		});

		return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}
