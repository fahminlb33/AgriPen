import { createContext } from "react";
import { useContext, useMemo } from "react";
import { useLocalStorage } from "@mantine/hooks";

export interface UserData {
	id: string;
	username: string;
	role: string;
	accessToken: string;
	refreshToken: string;
}

export interface AuthContext {
	user: UserData | null;
	login: (user: UserData | null) => void;
	logout: () => void;
}

export const AuthContext = createContext<AuthContext>({
	user: null,
	login: () => {},
	logout: () => {},
});

export function AuthProvider({ children }: { children: React.ReactNode }) {
	const [user, setUser] = useLocalStorage<UserData | null>({ key: "user" });

	function login(user: UserData | null) {
		setUser(user);
	}

	function logout() {
		setUser(null);
	}

	const memo = useMemo(() => ({ user, login, logout }), [user, login, logout]);
	return <AuthContext.Provider value={memo}>{children}</AuthContext.Provider>;
}

export function useAuth() {
	return useContext(AuthContext);
}
