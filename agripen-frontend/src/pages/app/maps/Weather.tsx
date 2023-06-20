import { Divider, Text } from "@mantine/core";
import { useLoaderData } from "react-router-dom";
import { MapContainer, TileLayer, Marker, Popup } from "react-leaflet";

import { MapsService } from "@/services/api";
import { WeatherItem } from "@/services/api/maps.types";
import { formatLongDateTime } from "@/services/helpers/formatter";

export type WeatherProps = {
	data: WeatherItem[];
};

export async function WeatherLoader() {
	const data = await MapsService.weather();
	return { data };
}

export function Weather() {
	const { data } = useLoaderData() as WeatherProps;
	const markers = data.map((item) => (
		<Marker position={[item.latitude, item.longitude]} key={item.id}>
			<Popup>
				<Text>Waktu: {formatLongDateTime(item.timestamp)}</Text>
				<Text>
					Kecamatan: <b>{item.kecamatan}</b>
				</Text>
				<Divider />
				<Text>
					Suhu: {item.temperatureLow}°C - {item.temperatureHigh}°C
				</Text>
				<Text>
					Kelembapan: {item.humidityLow}% - {item.humidityHigh}%
				</Text>
				<Text>Cuaca: {item.weather}</Text>
				<Text>Arah angin: {item.wind}</Text>
				<Text>Kecepatan angin: {item.windSpeed}</Text>
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
