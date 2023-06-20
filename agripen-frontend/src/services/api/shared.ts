export type DateRange = {
	startDate: Date;
	endDate: Date;
};

export type SortOrder = "Ascending" | "Descending";

export type SearchParams = {
	query?: string | null;
	page?: number;
	limit?: number;
	sortColumn?: string | null;
	sortDirection?: string | null;
};

export interface PaginationMeta {
	total: number;
	count: number;
	page: number;
	limit: number;
	totalPage: number;
}

export type GpsAddress = {
	latitude: number;
	longitude: number;
	altitude: number;
	horizontalAccuracy: number;
	verticalAccuracy: number;
	geocodedAddress?: string;
};

export type LocalAddress = {
	id: string;
	latitude: number;
	longitude: number;
	kecamatan: string;
	kabupaten: string;
	provinsi: string;
};

export const CLASS_NAME: Record<string, string> = {
	UNKNOWN: "-",
	BACTERIALBLIGHT: "Bacterial Blight",
	BROWNSPOT: "Brown Spot",
	BLAST: "Blast",
	TUNGRO: "Tungro",
	HEALTHY: "Sehat",
};
