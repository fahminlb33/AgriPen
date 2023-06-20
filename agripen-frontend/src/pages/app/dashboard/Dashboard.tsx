import { Suspense } from "react";
import {
	Flex,
	Group,
	Loader,
	SimpleGrid,
	Stack,
	Text,
	Title,
	createStyles,
} from "@mantine/core";
import { DatePickerInput, DatesRangeValue } from "@mantine/dates";
import {
	IconMapSearch,
	IconMinus,
	IconPlus,
	IconVirusSearch,
} from "@tabler/icons-react";
import { Await, defer, useLoaderData, useSearchParams } from "react-router-dom";

import {
	StatsGrid,
	DashboardPieChart,
} from "@/components/widgets/dashboard";
import { useAuth } from "@/hooks/useAuth";

import { getDefaultDateFilter, toDateRange } from "@/services/helpers/dates";
import { formatNumber, formatShortDate } from "@/services/helpers/formatter";
import { DashboardService } from "@/services/api";
import { CLASS_NAME } from "@/services/api/shared";
import {
	DiseaseTypeChart,
	DiseaseReportLocationChart,
	Summary,
} from "@/services/api/dashboard.types";

type DashboardProps = {
	summary: Summary;
	diseaseChart: DiseaseTypeChart[];
	diseaseLocationChart: DiseaseReportLocationChart[];
};

export async function DashboardLoader({ request }: { request: Request }) {
	const url = new URL(request.url);
	const startDate = url.searchParams.get("start_date");
	const endDate = url.searchParams.get("end_date");

	const date =
		toDateRange([new Date(startDate || ""), new Date(endDate || "")]) ??
		getDefaultDateFilter();

	const summary = DashboardService.summary(date);
	const diseaseChart = DashboardService.diseaseChart(date);
	const diseaseLocationChart = DashboardService.diseaseLocationChart(date);

	return defer({ summary, diseaseChart, diseaseLocationChart });
}

export function Dashboard() {
	// styles
	const { classes, theme } = useStyles();
	// username
	const user = useAuth();
	// route
	const [_, setSearchParams] = useSearchParams();
	const data = useLoaderData() as DashboardProps;

	const handleDateRangeChange = async (value: DatesRangeValue) => {
		// check if the user is in the middle of selecting a range
		if (value[0] && !value[1]) return;

		// check if the user is clearing the range
		const range = toDateRange(value);
		if (!range) {
			setSearchParams({});
			return;
		}

		// set the search params
		setSearchParams({
			start_date: formatShortDate(range.startDate),
			end_date: formatShortDate(range.endDate),
		});
	};

	const spinnerElement = (
		<Flex
			align={"center"}
			justify={"center"}
			wrap={"wrap"}
			direction={"column"}
		>
			<Loader />
			<Text>Memuat data...</Text>
		</Flex>
	);

	return (
		<div className={classes.mainContainer}>
			<Group position="apart" mb={`calc(${theme.spacing.xl} * 2)`}>
				<Stack spacing={0}>
					<Title order={2}>Halo, {user?.user?.username || "???"}.</Title>
					<Text>Selamat datang di AgriPen!</Text>
				</Stack>
				<DatePickerInput
					type="range"
					label="Filter berdasarkan tanggal"
					placeholder="Pilih rentang tanggal"
					valueFormat="DD MMMM YYYY"
					onChange={handleDateRangeChange}
					popoverProps={{ position: "bottom-end" }}
					w={230}
					clearable
					allowSingleDateInRange
				/>
			</Group>
			<SimpleGrid spacing={`calc(${theme.spacing.xl} * 1.5)`}>
				{/* Summary Statistics */}
				<Suspense fallback={spinnerElement}>
					<Await resolve={data.summary}>
						{(fd) => (
							<StatsGrid
								data={[
									{
										title: "Total Analisis Lahan",
										value: formatNumber(fd.landObservations ?? 0),
										icon: IconMapSearch,
									},
									{
										title: "Total Analisis Penyakit",
										value: formatNumber(fd.diseasePredictions ?? 0),
										icon: IconVirusSearch,
									},
									{
										title: "Total Deteksi Positif",
										value: formatNumber(fd.positiveDisease ?? 0),
										icon: IconPlus,
									},
									{
										title: "Total Deteksi Negatif",
										value: formatNumber(fd.negativeDisease ?? 0),
										icon: IconMinus,
									},
								]}
							/>
						)}
					</Await>
				</Suspense>

				<SimpleGrid cols={2}>
					{/* Statistik Analisis Penyakit */}
					<Suspense fallback={spinnerElement}>
						<Await resolve={data.diseaseChart}>
							{(fd) => (
								<DashboardPieChart
									data={fd.map((d: any) => ({
										name: CLASS_NAME[d.name.toUpperCase()],
										value: d.value,
									}))}
									title="Statistik Analisis Penyakit"
								/>
							)}
						</Await>
					</Suspense>

					{/* Statistik Laporan Penyakit per Desa */}
					<Suspense fallback={spinnerElement}>
						<Await resolve={data.diseaseLocationChart}>
							{(fd) => (
								<DashboardPieChart
									data={fd}
									title="Statistik Laporan per Daerah"
								/>
							)}
						</Await>
					</Suspense>
				</SimpleGrid>
			</SimpleGrid>
		</div>
	);
}

const useStyles = createStyles((theme) => ({
	mainContainer: {
		padding: theme.spacing.md,
	},
}));
