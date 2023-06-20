import {
	Group,
	Stack,
	Title,
	Text,
	SimpleGrid,
	createStyles,
	Button,
} from "@mantine/core";
import { IconCloud, IconMapSearch, IconVirusSearch } from "@tabler/icons-react";
import { Outlet, useNavigate } from "react-router-dom";

export function Home() {
	// styles
	const { classes, theme } = useStyles();
	//rotue
	const navigate = useNavigate();

	return (
		<div className={classes.mainContainer}>
			<Group position="apart" mb={`calc(${theme.spacing.xl} * 2)`}>
				<Stack spacing={0}>
					<Title order={2}>Pemetaan</Title>
					<Text>Pilih jenis analisis pemetaan.</Text>
				</Stack>
			</Group>
			<SimpleGrid cols={3}>
				<Button
					size="xl"
					radius="lg"
					variant="gradient"
					gradient={{ from: "indigo", to: "cyan" }}
					leftIcon={<IconCloud />}
					onClick={() => navigate("/app/maps/weather")}
				>
					Cuaca
				</Button>
				<Button
					size="xl"
					radius="lg"
					variant="gradient"
					gradient={{ from: "orange", to: "red" }}
					leftIcon={<IconVirusSearch />}
					onClick={() => navigate("/app/maps/disease-predictions")}
				>
					Penyakit
				</Button>
				<Button
					size="xl"
					radius="lg"
					variant="gradient"
					gradient={{ from: "teal", to: "lime", deg: 105 }}
					leftIcon={<IconMapSearch />}
					onClick={() => navigate("/app/maps/land-observations")}
				>
					Lahan
				</Button>
			</SimpleGrid>

			<div className={classes.mapContainer}>
				<Outlet />
			</div>
		</div>
	);
}

const useStyles = createStyles((theme) => ({
	mainContainer: {
		padding: theme.spacing.md,
	},

	mapContainer: {
		height: "600px",
		marginTop: theme.spacing.xl,
	},
}));
