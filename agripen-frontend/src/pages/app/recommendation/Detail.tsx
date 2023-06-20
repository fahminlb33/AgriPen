import {
	Group,
	Paper,
	SimpleGrid,
	Stack,
	Table,
	Text,
	Title,
	createStyles,
} from "@mantine/core";
import { Params, useLoaderData } from "react-router-dom";

import { ResultRow } from "@/components/widgets/ResultRow";
import { Recommendation } from "@/services/api/recommendation.types";
import { formatKgHa, formatPpm } from "@/services/helpers/formatter";
import { RecommendationService } from "@/services/api";

export type PlantDetailsProps = {
	data: Recommendation | null;
};

export async function DetailLoader({ params }: { params: Params }) {
	const data = await RecommendationService.getPlant(params.id ?? "");
	return { data: data };
}

export function Detail() {
	// styles
	const { classes, theme } = useStyles();
	// route
	const { data } = useLoaderData() as PlantDetailsProps;

	if (!data) {
		return (
			<div>
				<Title order={3}>Informasi Tanaman</Title>
				<Text>Tidak ada data.</Text>
			</div>
		);
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
				<Paper withBorder shadow="sm" p="lg">
        <Title order={3}>Informasi Tanaman</Title>
				<Stack w={400} spacing={0} mb="xl">
					<ResultRow
						label="Nama tanaman"
						value={`${data.name} (${data.nameID})`}
					/>
					<ResultRow label="Kategori musim" value={data.season.season} />
					<ResultRow
						label="Suhu siang hari"
						value={`${data.season.tempDayLow}°C - ${data.season.tempDayHigh}°C`}
					/>
					<ResultRow
						label="Suhu malam hari"
						value={`${data.season.tempNightLow}°C - ${data.season.tempNightHigh}°C`}
					/>
					<ResultRow
						label="Kelembapan udara"
						value={`${data.season.humidityLow}% - ${data.season.humidityHigh}%`}
					/>
					<ResultRow
						label="Kelembapan tanah"
						value={`${data.season.soilMoistureLow}% - ${data.season.soilMoistureHigh}%`}
					/>
				</Stack>

				<Title order={4} mt="sm">
					Informasi Nitrogen (N)
				</Title>
				<Table>
					<thead>
						<tr>
							<th>Nitrogen (ppm)</th>
							<th>Nitrogen (kg/ha)</th>
							<th>Catatan</th>
						</tr>
					</thead>
					<tbody>
						{data.nitrogen.map((x, i) => {
							return (
								<tr key={`nitrogen-${i}`}>
									<td>{formatPpm(x.nitrogen)}</td>
									<td>{formatKgHa(x.nitrogen)}</td>
									<td>{x.notes}</td>
								</tr>
							);
						})}
					</tbody>
				</Table>

				<Title order={4} mt="sm">
					Informasi Fosfor (<i>Phosporus</i>/P)
				</Title>
				<Table>
					<thead>
						<tr>
							<th>{`Kategori 1 (< ${formatPpm(3)})`}</th>
							<th>{`Kategori 2 (${formatPpm(3)} - ${formatPpm(5)})`}</th>
							<th>{`Kategori 3 (${formatPpm(6)} - ${formatPpm(12)})`}</th>
							<th>{`Kategori 4 (${formatPpm(13)} - ${formatPpm(39)})`}</th>
							<th>{`Kategori 5 (+${formatPpm(40)})`}</th>
							<th>Catatan</th>
						</tr>
					</thead>
					<tbody>
						{data.phosporus.map((x, i) => {
							return (
								<tr key={`phosporus-${i}`}>
									<td>
										{formatPpm(x.category1)} atau {formatKgHa(x.category1)}
									</td>
									<td>
										{formatPpm(x.category2)} atau {formatKgHa(x.category2)}
									</td>
									<td>
										{formatPpm(x.category3)} atau {formatKgHa(x.category3)}
									</td>
									<td>
										{formatPpm(x.category4)} atau {formatKgHa(x.category4)}
									</td>
									<td>
										{formatPpm(x.category5)} atau {formatKgHa(x.category5)}
									</td>
									<td>{x.notes}</td>
								</tr>
							);
						})}
					</tbody>
				</Table>

				<Title order={4} mt="sm">
					Informasi Kalium (<i>Potassium</i>/K)
				</Title>
				<Table>
					<thead>
						<tr>
							<th>{`Kategori 1 (< ${formatPpm(50)})`}</th>
							<th>{`Kategori 2 (${formatPpm(50)} - ${formatPpm(99)})`}</th>
							<th>{`Kategori 3 (${formatPpm(100)} - ${formatPpm(199)})`}</th>
							<th>{`Kategori 4 (${formatPpm(200)} - ${formatPpm(299)})`}</th>
							<th>{`Kategori 5 (+${formatPpm(300)})`}</th>
							<th>Catatan</th>
						</tr>
					</thead>
					<tbody>
						{data.phosporus.map((x, i) => {
							return (
								<tr key={`potassium-${i}`}>
									<td>
										{formatPpm(x.category1)} atau {formatKgHa(x.category1)}
									</td>
									<td>
										{formatPpm(x.category2)} atau {formatKgHa(x.category2)}
									</td>
									<td>
										{formatPpm(x.category3)} atau {formatKgHa(x.category3)}
									</td>
									<td>
										{formatPpm(x.category4)} atau {formatKgHa(x.category4)}
									</td>
									<td>
										{formatPpm(x.category5)} atau {formatKgHa(x.category5)}
									</td>
									<td>{x.notes}</td>
								</tr>
							);
						})}
					</tbody>
				</Table>

				<Title order={4} mt="sm">
					Informasi Keasaman Tanah (pH)
				</Title>
				<Table>
					<thead>
						<tr>
							<th>Optimal</th>
							<th>Minimum</th>
							<th>Catatan</th>
						</tr>
					</thead>
					<tbody>
						{data.ph.map((x, i) => {
							return (
								<tr key={`ph-${i}`}>
									<td>{x.optimal}</td>
									<td>{x.minimum}</td>
									<td>{x.notes}</td>
								</tr>
							);
						})}
					</tbody>
				</Table>
        </Paper>
			</SimpleGrid>
		</div>
	);
}

const useStyles = createStyles((theme) => ({
	mainContainer: {
		padding: theme.spacing.md,
	},
}));
