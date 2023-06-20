import {
	Divider,
	Grid,
	Group,
	Paper,
	SimpleGrid,
	Stack,
	Text,
	Title,
	Image,
	createStyles,
} from "@mantine/core";
import { Params, useLoaderData } from "react-router-dom";
import { MapContainer, TileLayer, Marker } from "react-leaflet";

import { ResultRow } from "@/components/widgets/ResultRow";
import { DiseasePredictionsService } from "@/services/api";
import { CLASS_NAME } from "@/services/api/shared";
import { Prediction } from "@/services/api/disease_predictions.types";

export type AnalysisProps = {
	data: Prediction;
};

export async function AnalysisLoader({ params }: { params: Params }) {
	const id = params.id;
	if (!id) {
		return { data: null };
	}

	const data = await DiseasePredictionsService.get(id);
	return { data };
}

export function Analysis() {
	// styles
	const { classes, theme } = useStyles();
	// route
	const { data } = useLoaderData() as AnalysisProps;

	const getFileName = (url: string) => {
		const split = url.split("/");
		const filename = split[split.length - 1];
		const name = filename.split(".")[0].toUpperCase();
		return name === "UNKNOWN" ? "ORIGINAL" : name;
	};

	return (
		<div className={classes.mainContainer}>
			{/* Title */}
			<Group position="apart" mb={`calc(${theme.spacing.xl} * 2)`}>
				<Stack spacing={0}>
					<Title order={2}>Analisis Penyakit</Title>
					<Text>
						Analisis entri data dengan kode <b>{data.id}</b>.
					</Text>
				</Stack>
			</Group>

			{/* Summary */}
			<SimpleGrid>
				{/* Summary */}
				<Paper p="md" shadow="sm">
					<Title order={3}>Analisis AI</Title>
					<Text color="dimmed">
						Analisis citra menggunakan <i>artificial intelligence.</i>
					</Text>
					<Divider mt={6} mb={12} />
					<Grid>
						{/* Image Analysis */}
						<Grid.Col span={3} mr="xl">
							<Title order={5} mb={4}>
								Analisis Citra
							</Title>
							<ResultRow
								label="Hasil"
								value={CLASS_NAME[data.result.toUpperCase()]}
								bold
							/>
							<ResultRow
								label="Probabilitas"
								value={`${Math.max(
									...Object.values(data.probabilities),
								).toFixed(2)}%`}
							/>
							<ResultRow
								label="Keparahan"
								value={data.result.toUpperCase() === "HEALTHY" ? "-" : `${data.severity.toFixed(2)}%`}
							/>
						</Grid.Col>

						{/* Probabilities */}
						<Grid.Col span={3} mr="xl">
							<Title order={5} mb={4}>
								Probabilitas
							</Title>
							{Object.entries(data.probabilities).map(([key, value]) => (
								<ResultRow
									label={CLASS_NAME[key.toUpperCase()]}
									value={`${value.toFixed(2)}%`}
									key={key}
								/>
							))}
						</Grid.Col>

						{/* GPS */}
						<Grid.Col span={3} mr="xl">
							<Title order={5} mb={4}>
								Data GPS
							</Title>
							<ResultRow
								label="Koordinat"
								value={`(${data.gpsAddress.latitude}, ${data.gpsAddress.longitude})`}
							/>
							<ResultRow
								label="Ketinggian"
								value={`${data.gpsAddress.altitude} m`}
							/>
							<ResultRow
								label="Akurasi Vertikal"
								value={`${data.gpsAddress.verticalAccuracy} m`}
							/>
							<ResultRow
								label="Akurasi Horizontal"
								value={`${data.gpsAddress.horizontalAccuracy} m`}
							/>
						</Grid.Col>

						{/* Address */}
						<Grid.Col span={3} mr="xl">
							<Title order={5} mb={4}>
								Lokasi Pengambilan Citra
							</Title>
							<ResultRow
								label="Kecamatan"
								value={data.localAddress.kecamatan}
							/>
							<ResultRow
								label="Kabupaten"
								value={data.localAddress.kabupaten}
							/>
							<ResultRow label="Provinsi" value={data.localAddress.provinsi} />
							<ResultRow
								label="Alamat"
								value={data.gpsAddress.geocodedAddress || "Tidak diketahui"}
							/>
						</Grid.Col>
					</Grid>
				</Paper>

				{/* Image Grid */}
				<Paper p="md" shadow="sm">
					<Title order={3}>Pengolahan Citra</Title>
					<Text color="dimmed">
						Citra dari proses <i>artificial intelligence.</i>
					</Text>
					<Divider mt={6} mb={12} />
					<Grid p="md">
						{data.images.map((image) => (
							<Grid.Col span={3} key={image}>
								<Image
									className={classes.lineBordered}
									src={image}
									fit="contain"
									caption={getFileName(image)}
								/>
							</Grid.Col>
						))}
					</Grid>
				</Paper>

				{/* Peta */}
				<Paper p="md" shadow="sm">
					<Title order={3}>Peta Lokasi</Title>
					<Text color="dimmed">Lokasi pengambilan gambar.</Text>
					<Divider mt={6} mb={12} />
					<MapContainer
						center={[data.gpsAddress.latitude, data.gpsAddress.longitude]}
						zoom={5}
						zoomControl={true}
						scrollWheelZoom={true}
						className={classes.mapContainer}
					>
						<TileLayer
							attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
							url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
						/>
						<Marker
							position={[data.gpsAddress.latitude, data.gpsAddress.longitude]}
						/>
					</MapContainer>
				</Paper>
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
