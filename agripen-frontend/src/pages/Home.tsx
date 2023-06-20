import { useDocumentTitle } from "@mantine/hooks";

import { HomeHero } from "@/components/widgets/home";

export function Home() {
	useDocumentTitle("AgriPen");

	return (
		<div style={{ marginTop: "3rem", marginBottom: "3rem" }}>
			<HomeHero />
		</div>
	);
}
