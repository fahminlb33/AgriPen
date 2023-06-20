import { AppShell, Container } from "@mantine/core";
import { Outlet } from "react-router-dom";

import { RootHeader, RootFooter } from "@/components/elements";

export default function PublicLayout() {
	return (
		<AppShell header={<RootHeader />} footer={<RootFooter />}>
			<Container>
				<Outlet />
			</Container>
		</AppShell>
	);
}
