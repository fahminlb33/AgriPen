import dayjs from "dayjs";

const LOCALE_ID = "id-ID";

export function formatNumber(n: number): string {
	return n.toLocaleString(LOCALE_ID, { style: "decimal" });
}

export function formatShortDate(date: Date): string {
	return dayjs(date).format("YYYY-MM-DD");
}

export function formatLongDate(date: Date): string {
	return dayjs(date).format("dddd, D MMMM YYYY");
}

export function formatLongDateTime(date: Date): string {
	return dayjs(date).format("dddd, D MMMM YYYY HH:mm:ss");
}

export function formatAlt1DateTime(date: Date): string {
	return dayjs(date).format("DD/MM/YYYY HH:mm:ss");
}

export function formatPpm(n: number): string {
  return `${formatNumber(n * 0.5)} ppm`;
}

export function formatKgHa(n: number): string {
  return `${formatNumber(n * 1.209)} kg/ha`;
}