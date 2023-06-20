import { useEffect, useState } from "react";
import {
	FileInput,
	Group,
	NumberInput,
	Paper,
	SimpleGrid,
	Stack,
	Text,
	Title,
	createStyles,
	Button,
} from "@mantine/core";
import { useForm } from "@mantine/form";
import { useGeolocated } from "react-geolocated";
import { useNavigate } from "react-router-dom";
import { IconGps, IconUpload } from "@tabler/icons-react";
import { DiseasePredictionsService } from "@/services/api";
import { notifications } from "@mantine/notifications";

export function Upload() {
	// styles
	const { classes, theme } = useStyles();
	// route
	const navigate = useNavigate();
	// states
	const [isLoading, setIsLoading] = useState(false);
	const [shouldUpdateGps, setShouldUpdateGps] = useState(false);
	const gps = useGeolocated({
		positionOptions: {
			enableHighAccuracy: true,
		},
		userDecisionTimeout: 5000,
	});

	const form = useForm({
		initialValues: {
			latitude: gps.coords?.latitude || 0.0,
			longitude: gps.coords?.longitude || 0.0,
			altitude: gps.coords?.altitude || 0.0,
			verticalAccuracy: gps.coords?.altitudeAccuracy || 0.0,
			horizontalAccuracy: gps.coords?.accuracy || 0.0,
			image: null,
		},

		validate: {
			latitude: (value) =>
				value + 90 >= 0
					? null
					: "Koordinat latitude harus berada pada -90 s.d. 90",
			longitude: (value) =>
				value + 180 >= 0
					? null
					: "Koordinat longitude harus berada pada -180 s.d. 180",
			altitude: (value) =>
				value >= 0 ? null : "Ketinggian harus bernilai positif",
			verticalAccuracy: (value) =>
				value >= 0 ? null : "Akurasi ketinggian harus bernilai positif",
			horizontalAccuracy: (value) =>
				value >= 0 ? null : "Akurasi horizontal harus bernilai positif",
			image: (value) => (value ? null : "Foto harus dipilih"),
		},
	});

	useEffect(() => {
		form.setValues({
			latitude: gps.coords?.latitude || 0.0,
			longitude: gps.coords?.longitude || 0.0,
			altitude: gps.coords?.altitude || 0.0,
			verticalAccuracy: gps.coords?.altitudeAccuracy || 0.0,
			horizontalAccuracy: gps.coords?.accuracy || 0.0,
		});
	}, [shouldUpdateGps]);

	function handleGpsUpdate() {
		gps.getPosition();
		setShouldUpdateGps(true);
	}

	async function handleUpload(data: typeof form.values) {
		setIsLoading(true);

		// get form
		const file = data.image as unknown as File;
		const gpsData = {
			latitude: data.latitude,
			longitude: data.longitude,
			altitude: data.altitude,
			verticalAccuracy: data.verticalAccuracy,
			horizontalAccuracy: data.horizontalAccuracy,
		};

		// upload
		const result = await DiseasePredictionsService.upload(file, gpsData);
		if (!result) {
			setIsLoading(false);
			notifications.show({
				title: "Gagal Unggah",
				message: "Gagal mengunggah ke server. Silakan coba lagi nanti.",
				color: "red",
			});
			return;
		}

		return navigate(`/app/disease-predictions/${result.id}`);
	}

	return (
		<div className={classes.mainContainer}>
			<Group position="apart" mb={`calc(${theme.spacing.xl} * 2)`}>
				<Stack spacing={0}>
					<Title order={2}>Unggah Foto</Title>
					<Text>Analisis foto penyakit tanaman padi.</Text>
				</Stack>
			</Group>
			<form onSubmit={form.onSubmit(handleUpload)}>
				<SimpleGrid cols={2}>
					<Paper p="md" shadow="sm">
						<NumberInput
							required
							hideControls
							label="Latitude"
							precision={4}
							disabled={isLoading}
							{...form.getInputProps("latitude")}
						/>
						<NumberInput
							required
							hideControls
							label="Longitude"
							precision={4}
							disabled={isLoading}
							{...form.getInputProps("longitude")}
						/>
						<NumberInput
							required
							hideControls
							label="Altitude"
							precision={4}
							disabled={isLoading}
							{...form.getInputProps("altitude")}
						/>
						<NumberInput
							required
							hideControls
							label="Akurasi vertikal"
							precision={4}
							disabled={isLoading}
							{...form.getInputProps("verticalAccuracy")}
						/>
						<NumberInput
							required
							hideControls
							label="Akurasi horizontal"
							precision={4}
							disabled={isLoading}
							{...form.getInputProps("horizontalAccuracy")}
						/>
						<FileInput
							required
							label="Pilih foto"
							placeholder="Pilih foto"
							accept="image/png,image/jpeg"
							disabled={isLoading}
							{...form.getInputProps("image")}
						/>
						<Group position="right" mt="md">
							<Button
								leftIcon={<IconGps />}
								variant="light"
								onClick={handleGpsUpdate}
								disabled={isLoading}
							>
								Lacak GPS
							</Button>
							<Button
								leftIcon={<IconUpload />}
								type="submit"
								disabled={isLoading}
							>
								Unggah
							</Button>
						</Group>
					</Paper>
				</SimpleGrid>
			</form>
		</div>
	);
}

const useStyles = createStyles((theme) => ({
	mainContainer: {
		padding: theme.spacing.md,
	},

	uploadImage: {
		maxHeight: "100%",
		maxWidth: "100%",
	},
}));
