import { useState } from "react";
import {
	TextInput,
	PasswordInput,
	Paper,
	Title,
	Text,
	Container,
	Group,
	Button,
	createStyles,
} from "@mantine/core";
import { notifications } from "@mantine/notifications";
import { useNavigate } from "react-router-dom";

import { useAuth } from "@/hooks/useAuth";

import { AuthService } from "@/services/api";

const useStyles = createStyles((theme) => ({
	title: {
		fontWeight: 900,
		fontFamily: `Greycliff CF, ${theme.fontFamily}`,
	},
}));

export function Login() {
	const { classes } = useStyles();
	const auth = useAuth();
	const navigate = useNavigate();

	const [user, setUser] = useState("");
	const [password, setPassword] = useState("");
	const [loading, setLoading] = useState(false);

	async function handleLogin() {
		// check if the form is filled
		if (!user || !password) {
			notifications.show({
				title: "Login gagal",
				message: "Username atau password salah",
				color: "red",
			});
			return;
		}

		setLoading(true);

		// login
		const loginRes = await AuthService.login(user, password);
		if (!loginRes) {
			notifications.show({
				title: "Login gagal",
				message: "Username atau password salah",
				color: "red",
			});
			return;
		}

		// set token
		auth.login({
			id: loginRes.userId,
			accessToken: loginRes.accessToken,
			refreshToken: loginRes.refreshToken,
			username: "",
			role: "",
		});

		// get profile
		const profile = await AuthService.profile();
		auth.login({
			id: loginRes.userId,
			accessToken: loginRes.accessToken,
			refreshToken: loginRes.refreshToken,
			username: profile?.username || "",
			role: profile?.role || "",
		});

		// navigate to dashboard
		navigate("/app/dashboard");
	}

	return (
		<Container size={420} my={40}>
			<Title align="center" className={classes.title}>
				Selamat datang!
			</Title>
			<Text color="dimmed" size="sm" align="center" mt={5}>
				Masuk ke AgriPen.
			</Text>

			<Paper withBorder shadow="md" p={30} mt={30} radius="md">
				<TextInput
					label="Username/email"
					required
					disabled={loading}
					value={user}
					onChange={(event) => setUser(event.currentTarget.value)}
				/>
				<PasswordInput
					label="Kata sandi"
					mt="md"
					required
					disabled={loading}
					value={password}
					onChange={(event) => setPassword(event.currentTarget.value)}
					onKeyUp={(event) => {
						if (event.key === "Enter") {
							handleLogin();
						}
					}}
				/>

				<Group position="apart" mt="xl">
					<Button fullWidth disabled={loading} onClick={handleLogin}>
						Masuk
					</Button>
				</Group>
			</Paper>
		</Container>
	);
}
