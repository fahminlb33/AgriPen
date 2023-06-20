import { AppShell } from "@mantine/core";
import { Outlet } from "react-router-dom";

export default function PublicLayout() {
	return (
		<AppShell>
			<Outlet />
		</AppShell>
	);
}
