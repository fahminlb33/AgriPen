export type UserProfile = {
	id: string;
	username: string;
	role: string;
	lastLoginAt: Date;
	createdAt: Date;
	updatedAt: Date;
};

export type LoginResponse = {
	userId: string;
	accessToken: string;
	refreshToken: string;
};
