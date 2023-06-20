import httpClient from "@/services/helpers/http_client";

import { SearchParams } from "./shared";
import {
	CreateRequest,
	ListResponse,
	ResetPasswordRequest,
	User,
} from "./users.types";

export async function create(req: CreateRequest): Promise<User | null> {
	try {
		const response = await httpClient.post("/users", req);
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

export async function get(id: string): Promise<User | null> {
	try {
		const response = await httpClient.get(`/users/${id}`);
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
		const response = await httpClient.get("/users", {
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
		await httpClient.delete(`/users/${id}`);
		return true;
	} catch (err) {
		console.error(err);
		return false;
	}
}

export async function resetPassword(
	req: ResetPasswordRequest,
): Promise<boolean> {
	try {
		await httpClient.put(`/users/${req.id}/password`, req);
		return true;
	} catch (err) {
		console.error(err);
		return false;
	}
}

export async function profile(): Promise<User | null> {
	try {
		const response = await httpClient.get("/users/profile");
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
