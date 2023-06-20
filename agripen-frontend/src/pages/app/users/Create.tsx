import { useState } from "react";
import {
	Group,
	Paper,
	SimpleGrid,
	Stack,
	Text,
	TextInput,
	Title,
	createStyles,
	Button,
	PasswordInput,
	Select,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { notifications } from "@mantine/notifications";
import { IconUserPlus } from "@tabler/icons-react";
import { useNavigate } from "react-router-dom";

import { UsersService } from "@/services/api";

export function Create() {
	// styles
	const { classes, theme } = useStyles();
	// route
	const navigate = useNavigate();
	// states
	const [isLoading, setIsLoading] = useState(false);

	const roleSelectData = [
		{ value: "administrator", label: "Admin" },
		{ value: "normal", label: "User" },
	];
	const allRoles = roleSelectData.map((item) => item.value);

	const form = useForm({
		initialValues: {
			username: "",
			email: "",
			password: "",
			role: "",
		},

		validate: {
			username: (value) =>
				value.length >= 5
					? null
					: "Username harus memiliki panjang minimal 5 karakter",
			email: (value) =>
				value.match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/) ? null : "Email tidak valid",
			password: (value) =>
				value.length >= 5
					? null
					: "Password harus memiliki panjang minimal 5 karakter",
			role: (value) => (allRoles.includes(value) ? null : "Role tidak valid"),
		},
	});

	async function handleCreate(data: typeof form.values) {
		setIsLoading(true);

		// get form
		const userData = {
			username: data.username,
			email: data.email,
			password: data.password,
			role: data.role,
		};

		// create user
		const result = await UsersService.create(userData);
		if (!result) {
			setIsLoading(false);
			notifications.show({
				title: "Gagal!",
				message: "Gagal membuat pengguna. Silakan coba lagi nanti.",
				color: "red",
			});
			return;
		}

		return navigate("/app/users");
	}

	return (
		<div className={classes.mainContainer}>
			<Group position="apart" mb={`calc(${theme.spacing.xl} * 2)`}>
				<Stack spacing={0}>
					<Title order={2}>Buat Pengguna</Title>
					<Text>Buat pengguna baru.</Text>
				</Stack>
			</Group>
			<form onSubmit={form.onSubmit(handleCreate)}>
				<SimpleGrid cols={2}>
					<Paper p="md" shadow="sm">
						<TextInput
							required
							label="Username"
							disabled={isLoading}
							{...form.getInputProps("username")}
						/>
						<TextInput
							required
							label="Email"
							disabled={isLoading}
							{...form.getInputProps("email")}
						/>
						<PasswordInput
							required
							label="Password"
							disabled={isLoading}
							{...form.getInputProps("password")}
						/>
						<Select
							required
							label="Role"
							data={roleSelectData}
							disabled={isLoading}
							{...form.getInputProps("role")}
						/>
						<Group position="right" mt="md">
							<Button
								leftIcon={<IconUserPlus />}
								type="submit"
								disabled={isLoading}
							>
								Buat Pengguna
							</Button>
						</Group>
					</Paper>
				</SimpleGrid>
			</form>
		</div>
	);
}

const useStyles = createStyles((theme) => ({
	mainContainer: {
		padding: theme.spacing.md,
	},
}));
