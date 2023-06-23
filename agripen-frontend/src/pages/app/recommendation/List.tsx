import { useEffect, useState } from "react";
import {
	Group,
	SimpleGrid,
	Stack,
	Text,
	Title,
	createStyles,
	Select,
	NumberInput,
	Button,
	Flex,
	ActionIcon,
	TextInput,
} from "@mantine/core";
import { notifications } from "@mantine/notifications";
import { DataTable, DataTableColumn } from "mantine-datatable";
import {
	IconEye,
	IconSearch,
} from "@tabler/icons-react";
import {
	useLoaderData,
	useNavigate,
} from "react-router-dom";

import { RecommendationService, LandObservationsService } from "@/services/api";
import {
	ListItem,
  SeasonItem,
} from "@/services/api/recommendation.types";
import { ListResponse } from "@/services/api/land_observations.types";
import { formatAlt1DateTime } from "@/services/helpers/formatter";

export type RecommendationProps = {
	seasons: SeasonItem[];
	observations: ListResponse;
};

export async function ListLoader() {
	const seasons = await RecommendationService.listSeasons();
	const observations = await LandObservationsService.list({
		page: 1,
		limit: 100,
	});

	return { seasons, observations };
}

export function List() {
	// styles
	const { classes, theme } = useStyles();
	// route
	const navigate = useNavigate();
	const data = useLoaderData() as RecommendationProps;
	const observationData = data.observations.data.map((x) => ({
		label: `${formatAlt1DateTime(x.createdAt)} (${x.id.substring(x.id.length - 8)})`,
		value: x.id,
	}));
	// states
	const [isLoading, setIsLoading] = useState(false);
	const [content, setContent] = useState<ListItem[] | null>([]);
	// --- find by plant name
	const [plantName, setPlantName] = useState("");
	// --- find by land observation
	const [landObservation, setLandObservation] = useState<string | null>("");
	// --- find by params
	const [season, setSeason] = useState<string | null>("");
	const [temperature, setTemperature] = useState<number | "">(0);
	const [airHumidity, setAirHumidity] = useState<number | "">(0);
	const [soilHumidity, setSoilHumidity] = useState<number | "">(0);

	const columns: DataTableColumn<ListItem>[] = [
		{ accessor: "name", title: "Nama Tanaman" },
		{
			accessor: "nameID",
			title: "Nama Tanaman (Inggris)",
		},
		{
			accessor: "season",
			title: "Musim",
		},
		{
			accessor: "actions",
			title: "Aksi",
			textAlignment: "right",
			render: (row) => (
				<Group spacing={4} position="right" noWrap>
					<ActionIcon
						color="green"
						onClick={() => navigate(`/app/recommendation/${row.id}`)}
					>
						<IconEye size={16} />
					</ActionIcon>
				</Group>
			),
		},
	];

	// after navigation, set loading to false
	useEffect(() => {
		setIsLoading(true);
		RecommendationService.findByPlant("")
			.then((res) => {
				setIsLoading(false);
				setContent(res ? res : []);
			})
			.catch((err) => {
				setIsLoading(false);
				notifications.show({
					title: "Analisis lahan",
					message: "Gagal menampilkan data tanaman!",
					color: "red",
				});
			});
	}, []);

	// --- event handlers
	function handleSearchByPlantName() {
		if (!plantName) {
			notifications.show({
				title: "Analisis lahan",
				message: "Masukkan nama tanaman terlebih dahulu!",
				color: "yellow",
			});
			return;
		}

		setIsLoading(true);
		RecommendationService.findByPlant(plantName)
			.then((res) => {
				setIsLoading(false);
				setContent(res ? res : []);
			})
			.catch((err) => {
				setIsLoading(false);
				notifications.show({
					title: "Analisis lahan",
					message: "Gagal menampilkan data tanaman!",
					color: "red",
				});
			});
	}

	function handleSearchByObservation() {
		if (!landObservation) {
			notifications.show({
				title: "Analisis lahan",
				message: "Pilih analisis lahan terlebih dahulu!",
				color: "yellow",
			});
			return;
		}

		setIsLoading(true);
		RecommendationService.findByObservations(landObservation)
			.then((res) => {
				setIsLoading(false);
				setContent(res ? res : []);
			})
			.catch((err) => {
				setIsLoading(false);
				notifications.show({
					title: "Analisis lahan",
					message: "Gagal menampilkan data tanaman!",
					color: "red",
				});
			});
	}

	function handleSearchByParams() {
		if (!season) {
			notifications.show({
				title: "Analisis lahan",
				message: "Pilih musim terlebih dahulu!",
				color: "yellow",
			});
			return;
		}

		setIsLoading(true);
		RecommendationService.findByParams({
			season_id: season,
			temperature: temperature,
			humidity: airHumidity,
			soil_moisture: soilHumidity,
		})
			.then((res) => {
				setIsLoading(false);
				setContent(res ? res : []);
			})
			.catch((err) => {
				setIsLoading(false);
				notifications.show({
					title: "Analisis lahan",
					message: "Gagal menampilkan data tanaman!",
					color: "red",
				});
			});
	}

	return (
		<div className={classes.mainContainer}>
			{/* Title */}
			<Group position="apart" mb={`calc(${theme.spacing.xl} * 2)`}>
				<Stack spacing={0}>
					<Title order={2}>Rekomendasi Lahan</Title>
					<Text>Cari rekomendasi perawatan tanaman.</Text>
				</Stack>
			</Group>

			{/* Content */}
			<SimpleGrid>
				{/* Search Parameters */}
				<Flex mb="xl">
					<Stack spacing={0} mr="xl">
						<Title order={5}>Cari berdasarkan Tanaman</Title>
						<TextInput
							label="Nama tanaman"
							w={300}
							value={plantName}
							onChange={(e) => setPlantName(e.currentTarget.value)}
							rightSection={
								<ActionIcon
									color={theme.primaryColor}
									variant="filled"
									onClick={handleSearchByPlantName}
								>
									<IconSearch size="1.1rem" stroke={1.5} />
								</ActionIcon>
							}
						/>
					</Stack>
					<Stack spacing={0} mr="xl">
						<Title order={5}>Cari berdasarkan Analisis Lahan</Title>
						<Select
							label="Analisis lahan"
							data={observationData}
							w={300}
							value={landObservation}
							onChange={setLandObservation}
							rightSection={
								<ActionIcon
									color={theme.primaryColor}
									variant="filled"
									onClick={handleSearchByObservation}
								>
									<IconSearch size="1.1rem" stroke={1.5} />
								</ActionIcon>
							}
						/>
					</Stack>
					<Stack spacing={0} mr="xl">
						<Title order={5}>Cari berdasarkan Karakteristik Lahan</Title>
						<Group>
							<Select
								label="Musim"
								data={data.seasons}
								w={120}
								value={season}
								onChange={setSeason}
							/>
							<NumberInput
								label="Suhu udara (°C)"
								w={120}
								min={-50}
								max={100}
								value={temperature}
								onChange={setTemperature}
							/>
							<NumberInput
								label="Kelembapan udara (%)"
								w={150}
								min={0}
								max={100}
								value={airHumidity}
								onChange={setAirHumidity}
							/>
							<NumberInput
								label="Kelembapan tanah (%)"
								w={150}
								min={0}
								max={100}
								value={soilHumidity}
								onChange={setSoilHumidity}
							/>
							<Button
								mt="xl"
								leftIcon={<IconSearch />}
								onClick={handleSearchByParams}
							>
								Cari
							</Button>
						</Group>
					</Stack>
				</Flex>

				{/* Search Results */}
				<DataTable
					withBorder
					// withColumnBorders
					highlightOnHover
					columns={columns}
					records={content ?? []}
					fetching={isLoading}
					minHeight={500}
				/>
			</SimpleGrid>
		</div>
	);
}

const useStyles = createStyles((theme) => ({
	mainContainer: {
		padding: theme.spacing.md,
	},

	lineBordered: {
		border: `2px solid ${theme.colors.gray[2]}`,
	},

	mapContainer: {
		height: "600px",
	},
}));
