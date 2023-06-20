import { Text, Title, List } from "@mantine/core";

export function About() {
	return (
		<div>
			<Title mt="xl" mb="xl">
				Tentang AgriPen
			</Title>
			<Text>
				AgriPen merupakan perkembangan dari aplikasi PadiScanner yang sebelumnya
				menjadi Juara 2 pada lomba <b>Bogor Innovation Award 2022</b>. Pada
				kesempatan ini peneliti ingin meningkatkan kapabilitas aplikasi
				pertanian cerdas berbasis <i>artificial intelligence</i> dengan
				menggabungkan data telemetri dari perangkat berbasis{" "}
				<i>internet of things</i> untuk membantu petani memilih jenis tanaman
				untuk dibudidaya.
			</Text>
			<Text mt={"sm"}>
				Secara umum, aplikasi ini memiliki semua kapabilitas dari aplikasi
				PadiScanner ditambah dengan penyandingan dengan perangkat sensor untuk
				mengumpulkan karakteristik lahan seperti suhu, kelembaban, dan
				penerangan matahari.
			</Text>
			<Text mt={"xl"}>Dikembangkan oleh:</Text>
			<List>
				<List.Item>Fahmi Noor Fiqri (S.II IPB University),</List.Item>
				<List.Item>
					Hanif Hanan Al-Jufri (S.I Universitas Pakuan), dan
				</List.Item>
				<List.Item>
					Abimanyu Okysaputra Rachman (S.I Universitas Pakuan).
				</List.Item>
			</List>
		</div>
	);
}
