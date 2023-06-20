import {
	AppShell,
	createStyles,
	Navbar,
	Group,
	getStylesRef,
	rem,
	Image,
	Switch,
	ColorScheme,
} from "@mantine/core";
import { useLocalStorage } from "@mantine/hooks";
import { Link, NavLink, Outlet } from "react-router-dom";
import { IconLogout, IconMoonStars, IconSun } from "@tabler/icons-react";

import LogoPng from "@/assets/img/logo.png";

import { APP_SIDEBAR_MENU } from "../routes/menus";

export default function AppLayout() {
	const { classes, theme, cx } = useStyles();

	const [colorScheme, setColorScheme] = useLocalStorage<ColorScheme>({
		key: "color-scheme",
		defaultValue: "light",
	});
	const toggleColorScheme = () =>
		setColorScheme((current) => (current === "dark" ? "light" : "dark"));

	const links = APP_SIDEBAR_MENU.map((item, i) => (
		<NavLink
			to={item.link}
			key={`sidebar-link-${item.label.toLowerCase()}-${i}`}
			className={(state) =>
				cx(classes.link, {
					[classes.linkActive]: state.isActive,
				})
			}
		>
			<item.icon className={classes.linkIcon} />
			<span>{item.label}</span>
		</NavLink>
	));

	return (
		<AppShell
			padding="md"
			navbar={
				<Navbar width={{ sm: 300 }} p="md">
					<Navbar.Section grow>
						<Group className={classes.header} position="apart">
							<Image src={LogoPng} width={28} height={28} />
							<Switch
								size="md"
								checked={colorScheme === "dark"}
								color={theme.colorScheme === "light" ? "grey" : "dark"}
								onLabel={
									<IconMoonStars size="1rem" color={theme.colors.blue[6]} />
								}
								offLabel={
									<IconSun size="1rem" color={theme.colors.yellow[7]} />
								}
								onClick={toggleColorScheme}
							/>
						</Group>
						{links}
					</Navbar.Section>

					<Navbar.Section className={classes.footer}>
						<Link
							to="#"
							className={classes.link}
							onClick={(event) => event.preventDefault()}
						>
							<IconLogout className={classes.linkIcon} />
							<span>Logout</span>
						</Link>
					</Navbar.Section>
				</Navbar>
			}
			styles={(theme) => ({
				main: {
					backgroundColor:
						theme.colorScheme === "dark"
							? theme.colors.dark[8]
							: theme.colors.gray[0],
				},
			})}
		>
			<Outlet />
		</AppShell>
	);
}

const useStyles = createStyles((theme) => ({
	header: {
		paddingBottom: theme.spacing.md,
		marginBottom: `calc(${theme.spacing.md} * 1.5)`,
		borderBottom: `${rem(1)} solid ${
			theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[2]
		}`,
	},

	footer: {
		paddingTop: theme.spacing.md,
		marginTop: theme.spacing.md,
		borderTop: `${rem(1)} solid ${
			theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[2]
		}`,
	},

	link: {
		...theme.fn.focusStyles(),
		display: "flex",
		alignItems: "center",
		textDecoration: "none",
		fontSize: theme.fontSizes.lg,
		color:
			theme.colorScheme === "dark"
				? theme.colors.dark[1]
				: theme.colors.gray[7],
		padding: `${theme.spacing.xs} ${theme.spacing.lg}`,
		borderRadius: theme.radius.sm,
		fontWeight: 500,

		"&:hover": {
			backgroundColor:
				theme.colorScheme === "dark"
					? theme.colors.dark[6]
					: theme.colors.gray[0],
			color: theme.colorScheme === "dark" ? theme.white : theme.black,

			[`& .${getStylesRef("icon")}`]: {
				color: theme.colorScheme === "dark" ? theme.white : theme.black,
			},
		},
	},

	linkIcon: {
		ref: getStylesRef("icon"),
		color:
			theme.colorScheme === "dark"
				? theme.colors.dark[2]
				: theme.colors.gray[6],
		marginRight: theme.spacing.lg,
	},

	linkActive: {
		"&, &:hover": {
			backgroundColor: theme.fn.variant({
				variant: "light",
				color: theme.primaryColor,
			}).background,
			color: theme.fn.variant({ variant: "light", color: theme.primaryColor })
				.color,
			[`& .${getStylesRef("icon")}`]: {
				color: theme.fn.variant({ variant: "light", color: theme.primaryColor })
					.color,
			},
		},
	},
}));
