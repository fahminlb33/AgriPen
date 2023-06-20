import httpClient from "@/services/helpers/http_client";

import { SearchParams } from "./shared";
import { ListResponse, Observation } from "./land_observations.types";

export async function get(id: string): Promise<Observation | null> {
	try {
		const response = await httpClient.get(`/land-observations/${id}`);

		if (response.status !== 200) {
			console.warn(response.data);
			return null;
		}

		return {
			...response.data,
			telemetry: response.data.telemetry.map((t: any) => ({
				...t,
				timestamp: new Date(t.timestamp),
			})),
			createdAt: new Date(response.data.createdAt),
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
		const response = await httpClient.get("/land-observations", {
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

export async function deleteItem(id: string): Promise<boolean> {
	try {
		await httpClient.delete(`/land-observations/${id}`);
		return true;
	} catch (err) {
		console.error(err);
		return false;
	}
}
