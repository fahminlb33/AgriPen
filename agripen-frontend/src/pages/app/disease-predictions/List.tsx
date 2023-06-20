import { Suspense, useEffect, useState } from "react";
import {
	ActionIcon,
	Button,
	Flex,
	Group,
	Loader,
	SimpleGrid,
	Stack,
	Text,
	TextInput,
	Title,
	createStyles,
} from "@mantine/core";
import { modals } from "@mantine/modals";
import {
	DataTable,
	DataTableColumn,
	DataTableSortStatus,
} from "mantine-datatable";
import {
	IconEye,
	IconSearch,
	IconTrash,
	IconUpload,
} from "@tabler/icons-react";
import {
	Await,
	defer,
	useLoaderData,
	useLocation,
	useNavigate,
	useSearchParams,
} from "react-router-dom";

import { PAGE_SIZES } from "@/services/helpers/table";
import { formatLongDate } from "@/services/helpers/formatter";
import { DiseasePredictionsService } from "@/services/api";
import { CLASS_NAME, SortOrder } from "@/services/api/shared";
import {
	ListItem,
	ListResponse,
} from "@/services/api/disease_predictions.types";

export type ListProps = {
	data: ListResponse;
	page: number;
	limit: number;
	sortBy: string;
	sortOrder: SortOrder;
};

export async function ListLoader({ request }: { request: Request }) {
	const url = new URL(request.url);
	const query = url.searchParams.get("q") || "";
	const page = parseInt(url.searchParams.get("page") || "1");
	const limit = parseInt(url.searchParams.get("limit") || "10");
	const sortBy = url.searchParams.get("sort_by") || "createdAt";
	const sortOrder = url.searchParams.get("sort_order") || "Ascending";

	const data = DiseasePredictionsService.list({
		query,
		page,
		limit,
		sortColumn: sortBy,
		sortDirection: sortOrder,
	});

	return defer({ data, page, limit, sortBy, sortOrder });
}

export function List() {
	// styles
	const { classes, theme } = useStyles();
	// route
	const [_, setQs] = useSearchParams();
	const location = useLocation();
	const navigate = useNavigate();
	const data = useLoaderData() as ListProps;
	// state
	const [isLoading, setIsLoading] = useState(true);
	const [search, setSearch] = useState("");

	// after navigation, set loading to false
	useEffect(() => {
		setIsLoading(false);
	}, [location]);

	function setQuery() {
		setIsLoading(true);
		setQs((prev) => {
			prev.set("q", search);
			return prev;
		});
	}

	function setPage(page: number) {
		setIsLoading(true);
		setQs((prev) => {
			prev.set("page", `${page}`);
			return prev;
		});
	}

	function setPageSize(limit: number) {
		setIsLoading(true);
		setQs((prev) => {
			prev.set("limit", `${limit}`);
			return prev;
		});
	}

	function setSortStatus(sortStatus: DataTableSortStatus) {
		setIsLoading(true);
		setQs((prev) => {
			prev.set("sort_by", `${sortStatus.columnAccessor}`);
			prev.set("sort_order", `${sortStatus.direction}`);
			return prev;
		});
	}

	async function handleDelete(id: string) {
		modals.openConfirmModal({
			title: "Hapus Prediksi Penyakit",
			children: (
				<Text size="sm">Apakah Anda yakin ingin menghapus data ini?</Text>
			),
			centered: true,
			confirmProps: { color: "red" },
			labels: { confirm: "Hapus", cancel: "Batal" },
			onConfirm: () => {
				DiseasePredictionsService.deleteItem(id).then(() => {
					navigate("/app/disease-predictions");
				});
			},
		});
	}

	const columns: DataTableColumn<ListItem>[] = [
		{ accessor: "address", title: "Lokasi", sortable: true },
		{ accessor: "status", title: "Status", sortable: true },
		{
			accessor: "result",
			title: "Hasil",
			sortable: true,
			render: (row) => CLASS_NAME[row.result.toUpperCase()],
		},
		{
			accessor: "probability",
			title: "Probabilitas",
			sortable: true,
			render: (row) => `${row.probability.toFixed(2)}%`,
		},
		{
			accessor: "severity",
			title: "Keparahan",
			sortable: true,
			render: (row) =>
				row.result.toUpperCase() === "HEALTHY"
					? "-"
					: `${(+row.severity * 100).toFixed(2)}%`,
		},
		{
			accessor: "createdAt",
			title: "Tanggal Diunggah",
			sortable: true,
			render: (row) => formatLongDate(row.createdAt),
		},
		{
			accessor: "actions",
			title: "Aksi",
			textAlignment: "right",
			render: (row) => (
				<Group spacing={4} position="right" noWrap>
					<ActionIcon
						color="green"
						onClick={() => navigate(`/app/disease-predictions/${row.id}`)}
					>
						<IconEye size={16} />
					</ActionIcon>
					<ActionIcon color="red" onClick={() => handleDelete(row.id)}>
						<IconTrash size={16} />
					</ActionIcon>
				</Group>
			),
		},
	];

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
					<Title order={2}>Analisis Penyakit</Title>
					<Text>Analisis foto penyakit tanaman padi.</Text>
				</Stack>
			</Group>
			<SimpleGrid>
				<Group spacing={0} position={"apart"}>
					<TextInput
						placeholder="Pencarian..."
						disabled={isLoading}
						rightSection={
							<ActionIcon
								color={theme.primaryColor}
								variant="filled"
								onClick={setQuery}
							>
								<IconSearch size="1.1rem" stroke={1.5} />
							</ActionIcon>
						}
						onChange={(e) => setSearch(e.currentTarget.value)}
						onKeyUp={(event) => {
							if (event.key === "Enter") {
								setQuery();
							}
						}}
					/>
					<Button
						leftIcon={<IconUpload />}
						onClick={() => navigate("/app/disease-predictions/upload")}
					>
						Unggah
					</Button>
				</Group>
				<Suspense fallback={spinnerElement}>
					<Await resolve={data.data}>
						{(tableData) => (
							<DataTable
								withBorder
								// withColumnBorders
								highlightOnHover
								columns={columns}
								records={tableData.data}
								totalRecords={tableData.meta.total}
								page={data.page}
								recordsPerPage={data.limit}
								recordsPerPageOptions={PAGE_SIZES}
								sortStatus={{
									columnAccessor: data.sortBy,
									direction: data.sortOrder === "Ascending" ? "asc" : "desc",
								}}
								onPageChange={setPage}
								onSortStatusChange={setSortStatus}
								onRecordsPerPageChange={setPageSize}
								fetching={isLoading}
								minHeight={500}
							/>
						)}
					</Await>
				</Suspense>
			</SimpleGrid>
		</div>
	);
}

const useStyles = createStyles((theme) => ({
	mainContainer: {
		padding: theme.spacing.md,
	},
}));
