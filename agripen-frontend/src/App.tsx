import dayjs from "dayjs";
import localizedFormat from "dayjs/plugin/localizedFormat";
import "dayjs/locale/id";

dayjs.extend(localizedFormat);
dayjs.locale("id");

import { ColorScheme, MantineProvider } from "@mantine/core";
import { DatesProvider } from "@mantine/dates";
import { Notifications } from "@mantine/notifications";
import { ModalsProvider } from "@mantine/modals";
import { useLocalStorage } from "@mantine/hooks";

import { RouterProvider } from "react-router-dom";

import modals from "@/components/modals";
import { AuthProvider } from "@/hooks/useAuth";

import router from "./routes";

function App() {
	const [colorScheme] = useLocalStorage<ColorScheme>({
		key: "color-scheme",
		defaultValue: "light",
	});

	return (
		<AuthProvider>
			<MantineProvider
				withGlobalStyles
				withNormalizeCSS
				theme={{ colorScheme }}
			>
				<ModalsProvider modals={modals}>
					<DatesProvider
						settings={{ locale: "id", firstDayOfWeek: 1, weekendDays: [6, 0] }}
					>
						<Notifications position="top-center" />
						<RouterProvider router={router} />
					</DatesProvider>
				</ModalsProvider>
			</MantineProvider>
		</AuthProvider>
	);
}

export default App;
