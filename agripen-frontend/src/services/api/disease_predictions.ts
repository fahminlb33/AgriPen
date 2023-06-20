import httpClient from "@/services/helpers/http_client";

import { GpsAddress, SearchParams } from "./shared";
import {
	UploadResponse,
	ListResponse,
	Prediction,
} from "./disease_predictions.types";

export async function get(id: string): Promise<Prediction | null> {
	try {
		const response = await httpClient.get(`/disease-predictions/${id}`);
		return {
			...response.data,
			createdAt: new Date(response.data.createdAt),
			updatedAt: new Date(response.data.updatedAt),
		};
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function list({
	query,
	page = 1,
	limit = 10,
	sortColumn = "createdAt",
	sortDirection = "Ascending",
}: SearchParams): Promise<ListResponse | null> {
	try {
		const response = await httpClient.get("/disease-predictions", {
			params: {
				q: query,
				sort_by: sortColumn,
				sort_dir: sortDirection,
				page,
				limit,
			},
		});

		return {
			...response.data,
			createdAt: new Date(response.data.createdAt),
		};
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function upload(
	image: File,
	address: GpsAddress,
): Promise<UploadResponse | null> {
	try {
		const response = await httpClient.postForm("/disease-predictions", {
			image: image,
			gpsAddress: JSON.stringify(address),
		});

    return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function deleteItem(id: string): Promise<boolean> {
	try {
		await httpClient.delete(`/disease-predictions/${id}`);
		return true;
	} catch (err) {
		console.error(err);
		return false;
	}
}
