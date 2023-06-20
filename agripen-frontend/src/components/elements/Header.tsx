import {
	createStyles,
	Header,
	Group,
	Container,
	Burger,
	Button,
	Transition,
	Paper,
	Switch,
	useMantineTheme,
	ColorScheme,
} from "@mantine/core";
import { useDisclosure, useLocalStorage } from "@mantine/hooks";
import { IconBolt, IconMoonStars, IconSun } from "@tabler/icons-react";
import { Link, NavLink } from "react-router-dom";

import { PUBLIC_NAVBAR_MENU as links } from "@/routes/menus";

export function RootHeader() {
	const [opened, { toggle }] = useDisclosure(false);
	const { classes, cx } = useStyles();
	const theme = useMantineTheme();

	const [colorScheme, setColorScheme] = useLocalStorage<ColorScheme>({
		key: "color-scheme",
		defaultValue: "light",
	});
	const toggleColorScheme = () =>
		setColorScheme((current) => (current === "dark" ? "light" : "dark"));

	const items = links.map((link) => (
		<NavLink
			key={link.label}
			to={link.link}
			className={(state) =>
				cx(classes.link, {
					[classes.linkActive]: state.isActive,
				})
			}
		>
			{link.label}
		</NavLink>
	));

	return (
		<Header height={56} mb={60}>
			<Container className={classes.inner}>
				<Burger
					opened={opened}
					onClick={toggle}
					size="sm"
					className={classes.burger}
				/>
				<Group className={classes.links} spacing={5} w="auto">
					{items}
				</Group>

				<Group spacing={5}>
					<Switch
						size="md"
						checked={colorScheme === "dark"}
						color={theme.colorScheme === "light" ? "grey" : "dark"}
						onLabel={<IconMoonStars size="1rem" color={theme.colors.blue[6]} />}
						offLabel={<IconSun size="1rem" color={theme.colors.yellow[7]} />}
						onClick={toggleColorScheme}
					/>
				</Group>

				<Link to="/app">
					<Button
						className={classes.hideIfSmall}
						radius="xl"
						sx={{ height: 30 }}
					>
						<IconBolt style={{ marginRight: 10 }} /> AgriPen
					</Button>
				</Link>

				<Transition transition="pop-top-right" duration={200} mounted={opened}>
					{(styles) => (
						<Paper className={classes.dropdown} withBorder style={styles}>
							{items}
						</Paper>
					)}
				</Transition>
			</Container>
		</Header>
	);
}

const useStyles = createStyles((theme) => ({
	inner: {
		display: "flex",
		justifyContent: "space-between",
		alignItems: "center",
		height: 56,

		[theme.fn.smallerThan("sm")]: {
			justifyContent: "flex-start",
		},
	},

	links: {
		width: 260,

		[theme.fn.smallerThan("sm")]: {
			display: "none",
		},
	},

	hideIfSmall: {
		[theme.fn.smallerThan("sm")]: {
			display: "none",
		},
	},

	burger: {
		marginRight: theme.spacing.md,

		[theme.fn.largerThan("sm")]: {
			display: "none",
		},
	},

	link: {
		display: "block",
		lineHeight: 1,
		padding: "8px 12px",
		borderRadius: theme.radius.sm,
		textDecoration: "none",
		color:
			theme.colorScheme === "dark"
				? theme.colors.dark[0]
				: theme.colors.gray[7],
		fontSize: theme.fontSizes.sm,
		fontWeight: 500,

		"&:hover": {
			backgroundColor:
				theme.colorScheme === "dark"
					? theme.colors.dark[6]
					: theme.colors.gray[0],
		},
	},

	linkActive: {
		"&, &:hover": {
			backgroundColor: theme.fn.variant({
				variant: "light",
				color: theme.primaryColor,
			}).background,
			color: theme.fn.variant({ variant: "light", color: theme.primaryColor })
				.color,
		},
	},

	dropdown: {
		position: "absolute",
		top: 60,
		left: 0,
		right: 0,
		zIndex: 0,
		borderTopRightRadius: 0,
		borderTopLeftRadius: 0,
		borderTopWidth: 0,
		overflow: "hidden",

		[theme.fn.largerThan("sm")]: {
			display: "none",
		},
	},
}));
