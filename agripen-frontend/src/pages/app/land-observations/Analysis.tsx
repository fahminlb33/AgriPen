import dayjs from "dayjs";
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
import { MapContainer, Marker, TileLayer } from "react-leaflet";
import { meanBy } from "lodash-es";
import {
	CartesianGrid,
	Legend,
	Line,
	LineChart,
	ResponsiveContainer,
	Tooltip,
	XAxis,
	YAxis,
} from "recharts";

import { Observation } from "@/services/api/land_observations.types";
import { LandObservationsService } from "@/services/api";

export type AnalysisProps = {
	data: Observation;
};

export async function AnalysisLoader({ params }: { params: Params }) {
	const id = params.id;
	if (!id) {
		return { data: null };
	}

	const data = await LandObservationsService.get(id);
	return { data };
}

export function Analysis() {
	// styles
	const { classes, theme } = useStyles();
	// route
	const { data } = useLoaderData() as AnalysisProps;

	const ResultRow = ({
		label,
		value,
		bold,
	}: { label: string; value: string; bold?: boolean }) => (
		<Group position="apart">
			<Text>{label}</Text>
			{bold ? <Text fw={700}>{value}</Text> : <Text>{value}</Text>}
		</Group>
	);

	function formatXTick(value: any) {
		return dayjs(value).format("hh:mm:ss");
	}

	return (
		<div className={classes.mainContainer}>
			{/* Title */}
			<Group position="apart" mb={`calc(${theme.spacing.xl} * 2)`}>
				<Stack spacing={0}>
					<Title order={2}>Analisis Lahan</Title>
					<Text>
						Analisis entri data dengan kode <b>{data.id}</b>.
					</Text>
				</Stack>
			</Group>

			{/* Summary */}
			<SimpleGrid>
				{/* Summary */}
				<Paper p="md" shadow="sm">
					<Title order={3}>Analisis Data</Title>
					<Text color="dimmed">
						Analisis citra menggunakan <i>artificial intelligence.</i>
					</Text>
					<Divider mt={6} mb={12} />
					<Grid columns={12}>
						{/* Image Analysis */}
						<Grid.Col span={3} mr="xl">
							<Title order={5} mb={4}>
								Kondisi Lahan
							</Title>
							<ResultRow
								label="Rata-Rata Suhu Udara"
								value={`${meanBy(
									data.telemetry,
									(o) => o.airTemperature,
								).toFixed(2)}°C`}
							/>
							<ResultRow
								label="Rata-Rata Kelembapan Udara"
								value={`${meanBy(data.telemetry, (o) => o.airHumidity).toFixed(
									2,
								)}%`}
							/>
							<ResultRow
								label="Rata-Rata Heat Index"
								value={`${meanBy(
									data.telemetry,
									(o) => o.airHeatIndex,
								).toFixed(2)}°C`}
							/>
							<ResultRow
								label="Rata-Rata Kelembapan Tanah"
								value={`${meanBy(data.telemetry, (o) => o.soilMoisture).toFixed(
									2,
								)}%`}
							/>
							<ResultRow
								label="Rata-Rata Penyinaran Matahari"
								value={`${meanBy(
									data.telemetry,
									(o) => o.sunIllumination,
								).toFixed(2)}%`}
							/>
						</Grid.Col>

						{/* Address */}
						<Grid.Col span={3} mr="xl">
							<Title order={5} mb={4}>
								Lokasi Pengambilan Data
							</Title>
							<ResultRow
								label="Koordinat"
								value={`(${data.localAddress.latitude}, ${data.localAddress.longitude})`}
							/>
							<ResultRow
								label="Kecamatan"
								value={data.localAddress.kecamatan}
							/>
							<ResultRow
								label="Kabupaten"
								value={data.localAddress.kabupaten}
							/>
							<ResultRow label="Provinsi" value={data.localAddress.provinsi} />
						</Grid.Col>

						{/* GPS */}
						<Grid.Col span={4} mr="xl">
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
							<ResultRow
								label="Alamat"
								value={data.gpsAddress.geocodedAddress || "Tidak diketahui"}
							/>
						</Grid.Col>
					</Grid>
				</Paper>

				{/* Telemetry */}
				<Paper p="md" shadow="sm">
					<Title order={3}>Telemetri</Title>
					<Text color="dimmed">Data telemetri dari AgriPen.</Text>
					<Divider mt={6} mb={12} />
					<SimpleGrid cols={2} h={400}>
						{/* Physical Attributes */}
						<ResponsiveContainer>
							<LineChart data={data.telemetry}>
								<CartesianGrid strokeDasharray="3 3" />
								<XAxis dataKey="timestamp" tickFormatter={formatXTick} />
								<YAxis />
								<Tooltip />
								<Legend />
								<Line
									dataKey="airTemperature"
									name="Suhu Udara"
									stroke={theme.colors.cyan[7]}
								/>
								<Line
									dataKey="soilTemperature"
									name="Suhu Tanah"
									stroke={theme.colors.lime[7]}
								/>
							</LineChart>
						</ResponsiveContainer>

						{/* Percentage Attributes */}
						<ResponsiveContainer>
							<LineChart data={data.telemetry}>
								<CartesianGrid strokeDasharray="3 3" />
								<XAxis dataKey="timestamp" tickFormatter={formatXTick} />
								<YAxis />
								<Tooltip />
								<Legend />
								<Line
									dataKey="airHumidity"
									name="Kelembapan Udara"
									stroke={theme.colors.cyan[7]}
								/>
								<Line
									dataKey="soilMoisture"
									name="Kelembapan Tanah"
									stroke={theme.colors.lime[7]}
								/>
								<Line
									dataKey="sunIllumination"
									name="Penyinaran Matahari"
									stroke={theme.colors.yellow[7]}
								/>
							</LineChart>
						</ResponsiveContainer>
					</SimpleGrid>
				</Paper>

				{/* Image Grid */}
				<Paper p="md" shadow="sm">
					<Title order={3}>Foto Lahan</Title>
					<Text color="dimmed">Foto lokasi yang diobservasi.</Text>
					<Divider mt={6} mb={12} />
					<Grid p="md">
						{data.images.length > 0 ? (
							data.images.map((image) => (
								<Grid.Col span={3} key={image}>
									<Image
										className={classes.lineBordered}
										src={image}
										fit="contain"
									/>
								</Grid.Col>
							))
						) : (
							<Text>Tidak ada foto.</Text>
						)}
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
