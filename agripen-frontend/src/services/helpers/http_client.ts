import axios from "axios";

// adapted from:
// https://thedutchlab.com/blog/using-axios-interceptors-for-refreshing-your-api-token
// https://levelup.gitconnected.com/the-ultimate-guide-for-implementing-refresh-token-with-axios-bad47d0bfa05
// ref:
// https://vitejs.dev/guide/env-and-mode.html

const ANONYMOUS_API_ROUTES = [
	"/auth/login",
	"/auth/refresh-token",
];

// refresh token
export async function refreshToken(): Promise<boolean> {
	// get the refresh token from local storage
	const user = JSON.parse(localStorage.getItem("user") || "{}");

	if (!user.id || !user.refreshToken) {
		return false;
	}

	// make a request to the refresh token endpoint
	const response = await instance.post("/auth/refresh-token", {
		refreshToken: user.refreshToken,
		userId: user.id,
	});

	// if the response is not successful, reject the promise
	if (response.status !== 200) {
		return false;
	}

	// store the new tokens in local storage
	localStorage.setItem(
		"user",
		JSON.stringify({
			...user,
			accessToken: response.data.accessToken,
			refreshToken: response.data.refreshToken,
		}),
	);

	return true;
}

// create an axios instance
const instance = axios.create({
	baseURL: import.meta.env.VITE_API_BASE_URI,
});

// inject token to header if exist
instance.interceptors.request.use((config) => {
	// check if the endpoints is in anonymous mode
	if (ANONYMOUS_API_ROUTES.includes(config.url ?? "")) {
		return config;
	}

	// get the access token from local storage
	const user = JSON.parse(localStorage.getItem("user") || "{}");
	if (user.accessToken) {
		// inject the access token to the request header
		config.headers.setAuthorization(`Bearer ${user.accessToken}`);
	}

	return config;
});

// intercept expired access token
instance.interceptors.response.use(
	(response) => response,
	async (error) => {
		// if the response is 403 Forbidden, it's a token expired error, and this is not a retry
		if (error.response.status === 401 && !error.config._retry) {
			// set the retry flag so we don't retry the request again
			error.config._retry = true;

			// refresh the token
			if (await refreshToken()) {
				// retry the request
				const user = JSON.parse(localStorage.getItem("user") || "{}");
				error.config.headers.setAuthorization(`Bearer ${user.accessToken}`);

				return instance(error.config);
			}
		}

		// if the response is 401 Unauthorized, it's a token invalid error
		if (error.response.status === 401 && error.config._retry) {
			// clear the local storage and dispatch an event to notify other tabs
			localStorage.clear();
			window.dispatchEvent(new StorageEvent("storage"));

			// reject the promise with the error
			return error;
		}

		return error;
	},
);

export default instance;
