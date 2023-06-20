import { useState } from "react";
import {
	Button,
	Flex,
	Group,
	PasswordInput,
	Text,
	TextInput,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { ContextModalProps } from "@mantine/modals";
import { notifications } from "@mantine/notifications";

import { UsersService } from "@/services/api";

export type ChangePasswordModalProps = {
	id: string;
	username: string;
};

export function ChangePasswordModal({
	context,
	id,
	innerProps,
}: ContextModalProps<ChangePasswordModalProps>) {
	// states
	const [isLoading, setIsLoading] = useState(false);

	const form = useForm({
		initialValues: {
			password: "",
			confirmPassword: "",
		},

		validate: {
			password: (value) =>
				value.length >= 5
					? null
					: "Password harus memiliki panjang minimal 5 karakter",
			confirmPassword: (value, values) =>
				value === values.confirmPassword
					? null
					: "Konfirmasi password tidak sama dengan password",
		},
	});

	async function handleSubmit(data: typeof form.values) {
		setIsLoading(true);

		// get form
		const userData = {
			id: innerProps.id,
			newPassword: data.password,
		};

		// create user
		const result = await UsersService.resetPassword(userData);
		if (!result) {
			setIsLoading(false);
			notifications.show({
				title: "Gagal!",
				message: "Gagal mengganti password pengguna. Silakan coba lagi nanti.",
				color: "red",
			});
			return;
		}

		notifications.show({
			title: "Sukses!",
			message: "Password berhasil diganti.",
			color: "green",
		});
		context.closeModal(id);
	}

	return (
		<div>
			<TextInput label="Username" value={innerProps.username} readOnly />
			<form onSubmit={form.onSubmit(handleSubmit)}>
				<PasswordInput
					required
					label="Password Baru"
					disabled={isLoading}
					{...form.getInputProps("password")}
				/>
				<PasswordInput
					required
					label="Ulangi Password"
					disabled={isLoading}
					{...form.getInputProps("confirmPassword")}
				/>

				<Group position="right" mt="md">
					<Button
						color="red"
						disabled={isLoading}
						onClick={() => context.closeModal(id)}
					>
						Tutup
					</Button>
					<Button type="submit" disabled={isLoading}>
						Ganti Password
					</Button>
				</Group>
			</form>
		</div>
	);
}
