import { DatesRangeValue } from "@mantine/dates";

import { DateRange } from "@/services/api/shared";

export function toDateRange(r: DatesRangeValue): DateRange | undefined {
	if (r[0] && !isNaN(r[0].valueOf()) && r[1] && !isNaN(r[1].valueOf())) {
		return {
			startDate: r[0],
			endDate: r[1],
		};
	}
}

export function getDefaultDateFilter(): DateRange {
	const today = new Date();
	return {
		startDate: new Date(today.getFullYear(), today.getMonth(), 1),
		endDate: today,
	};
}
