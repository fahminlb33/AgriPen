import { Divider, Text } from "@mantine/core";
import { useLoaderData } from "react-router-dom";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";

import { MapsService } from "@/services/api";
import { DiseaseItem } from "@/services/api/maps.types";
import { formatLongDateTime } from "@/services/helpers/formatter";

export type DiseaseProps = {
	data: DiseaseItem[];
};

export async function DiseasePredictionsLoader() {
	const data = await MapsService.disease();
	return { data };
}

export function DiseasePredictions() {
	const { data } = useLoaderData() as DiseaseProps;
	const markers = data.map((item) => (
		<Marker position={[item.latitude, item.longitude]} key={item.id}>
			<Popup>
				<Text>Waktu: {formatLongDateTime(item.timestamp)}</Text>
				<Text>
					Alamat: <b>{item.address}</b>
				</Text>
				<Divider />
				<Text>
					Hasil Prediksi: {item.result} ({item.probability.toFixed(2)}%)
				</Text>
				<Text>Keparahan: {item.severity.toFixed(2)}%</Text>
				<Text>Status: {item.status}</Text>
			</Popup>
		</Marker>
	));

	return (
		<MapContainer
			center={[-6.5301876, 106.7855427]}
			zoom={11}
			zoomControl={true}
			scrollWheelZoom={true}
			style={{ height: "600px" }}
		>
			<TileLayer
				attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
				url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
			/>
			{markers}
		</MapContainer>
	);
}
