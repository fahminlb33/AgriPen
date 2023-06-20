import dayjs from "dayjs";

import { DateRange } from "./shared";
import {
	DiseaseReportLocationChart,
	DiseaseTypeChart,
	Summary,
} from "./dashboard.types";

import httpClient from "@/services/helpers/http_client";

export async function summary(dates: DateRange): Promise<Summary | null> {
	try {
		const response = await httpClient.get("/dashboard/summary", {
			params: {
				start_date: dayjs(dates.startDate).format("YYYY-MM-DD"),
				end_date: dayjs(dates.endDate).format("YYYY-MM-DD"),
			},
		});

		return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function diseaseChart(
	dates: DateRange,
): Promise<DiseaseTypeChart[] | null> {
	try {
		const response = await httpClient.get("/dashboard/disease-category-chart", {
			params: {
				start_date: dayjs(dates.startDate).format("YYYY-MM-DD"),
				end_date: dayjs(dates.endDate).format("YYYY-MM-DD"),
			},
		});

		return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}

export async function diseaseLocationChart(
	dates: DateRange,
): Promise<DiseaseReportLocationChart[] | null> {
	try {
		const response = await httpClient.get("/dashboard/disease-location-chart", {
			params: {
				start_date: dayjs(dates.startDate).format("YYYY-MM-DD"),
				end_date: dayjs(dates.endDate).format("YYYY-MM-DD"),
			},
		});

		return response.data;
	} catch (err) {
		console.error(err);
		return null;
	}
}
