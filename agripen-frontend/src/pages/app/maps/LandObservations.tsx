import { Divider, Text } from "@mantine/core";
import { useLoaderData } from "react-router-dom";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";

import { MapsService } from "@/services/api";
import { LandItem } from "@/services/api/maps.types";
import { formatLongDateTime } from "@/services/helpers/formatter";

export type LandObservationProps = {
	data: LandItem[];
};

export async function LandObservationsLoader() {
	const data = await MapsService.land();
	return { data };
}

export function LandObservations() {
	const { data } = useLoaderData() as LandObservationProps;
	const markers = data.map((item) => (
		<Marker position={[item.latitude, item.longitude]} key={item.id}>
			<Popup>
				<Text>Waktu: {formatLongDateTime(item.timestamp)}</Text>
				<Text>
					Alamat: <b>{item.address}</b>
				</Text>
				<Divider />
				<Text>Suhu udara: {item.airTemperature.toFixed(2)}°C</Text>
				<Text>Kelembapan udara: {item.airHumidity.toFixed(2)}%</Text>
				<Text><i>Heat index</i>: {item.airHeatIndex.toFixed(2)}°C</Text>
				<Text>Kelembapan tanah: {item.soilMoisture.toFixed(2)}%</Text>
				<Text>Penyinaran matahari: {item.sunIllumination.toFixed(2)} lux</Text>
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
