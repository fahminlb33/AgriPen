import { PaginationMeta } from "./shared";

export type UserRole = "administrator" | "normal";

export type ListItem = {
	id: string;

	username: string;
	role: string;

	lastLoginAt: Date;
	createdAt: Date;
};

export type ListResponse = {
	meta: PaginationMeta;
	data: ListItem[];
};

export type User = ListItem & {
	updatedAt: Date;
};

export type CreateRequest = {
	username: string;
	role: string;
	email: string;
	password: string;
};

export type ResetPasswordRequest = {
	id: string;
	newPassword: string;
};
