import { ContextModalProps } from "@mantine/modals";
import { Text, Button, List } from "@mantine/core";

export function HelpLandObservationModal({
	context,
	id,
	innerProps,
}: ContextModalProps<{}>) {
	return (
		<>
			<Text mb="sm">
				Tutorial untuk pengguna <b>Android.</b>
			</Text>
			<div style={{ marginRight: "5px" }}>
				<List type="ordered">
					<List.Item>
						Install aplikasi <b>AgriPen Mobile</b> dan masuk menggunakan akun
						Anda.
					</List.Item>
					<List.Item>
						Buka penutup atas sensor AgriPen, kemudian tancapkan ke tanah sesuai
						garis batas.
					</List.Item>
					<List.Item>Nyalakan AgriPen, kemudian tunggu 10 detik.</List.Item>
					<List.Item>
						Buka aplikasi AgriPen Mobile, kemudian tekan tombol{" "}
						<b>Analisis Lahan</b>.
					</List.Item>
					<List.Item>
						Pastikan Anda sudah memasangkan perangkat Anda dengan AgriPen
						Mobile, kemudian tekan <b>Mulai</b>.
					</List.Item>
					<List.Item>
						Tunggu proses observasi selesai (biasanya 1 menit). Jangan bergerak
						terlalu jauh dari alat!
					</List.Item>
					<List.Item>
						Setelah observasi selesai, AgriPen Mobile akan memberikan notifikasi
						proses dan Anda dapat membuka laman AgriPen untuk melihat data
						observasi terbaru.
					</List.Item>
					<List.Item>
						Cabut AgriPen dari tanah, matikan daya, bersihkan sensor, dan tutup
						kembali sebelum disimpan.
					</List.Item>
				</List>
			</div>

			<Button
				fullWidth
				color="red"
				mt="md"
				onClick={() => context.closeModal(id)}
			>
				Tutup
			</Button>
		</>
	);
}
