import { defineConfig } from "vite";

import react from "@vitejs/plugin-react-swc";
import { ViteImageOptimizer } from "vite-plugin-image-optimizer";

export default defineConfig({
	plugins: [react(), ViteImageOptimizer()],
	resolve: {
		alias: {
			"@": "/src",
		},
	},
	server: {
		port: 4000,
	},
	build: {
		sourcemap: false,
		rollupOptions: {
			output: {
				manualChunks: {
					base: ["react", "react-dom", "react-router-dom", "react-geolocated"],
					ui: [
						"@emotion/react",
						"@mantine/core",
						"@mantine/dates",
						"@mantine/hooks",
						"@mantine/modals",
						"@mantine/notifications",
					],
					widgets: ["@tabler/icons-react", "mantine-datatable", "recharts"],
					maps: ["leaflet", "react-leaflet"],
				},
			},
		},
	},
});
