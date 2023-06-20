import { LoginResponse, UserProfile } from "./auth.types";

import httpClient from "@/services/helpers/http_client";

export async function login(
	user: string,
	password: string,
): Promise<LoginResponse | null> {
	try {
		const response = await httpClient.post("/auth/login", {
			user: user,
			password: password,
		});

		if (response.status !== 200) {
			console.warn(response.data);
			return null;
		}

		return {
			...response.data,
			lastLoginAt: new Date(response.data.lastLoginAt),
			createdAt: new Date(response.data.createdAt),
			updatedAt: new Date(response.data.updatedAt),
		};
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function profile(): Promise<UserProfile | null> {
	try {
		const response = await httpClient.get("/users/profile");

		if (response.status !== 200) {
			console.warn(response.data);
			return null;
		}

		return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}
