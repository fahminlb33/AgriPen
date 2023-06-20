import {
	createStyles,
	Title,
	Text,
	Button,
	Container,
	rem,
	useMantineTheme,
} from "@mantine/core";
import { IconBolt } from "@tabler/icons-react";
import { useNavigate } from "react-router-dom";

import { HomeHeroDots } from "./Dots";

export function HomeHero() {
	const { classes } = useStyles();
	const theme = useMantineTheme();
	const navigate = useNavigate();

	return (
		<Container className={classes.wrapper} size={1400}>
			<HomeHeroDots className={classes.dots} style={{ left: 0, top: 0 }} />
			<HomeHeroDots className={classes.dots} style={{ left: 60, top: 0 }} />
			<HomeHeroDots className={classes.dots} style={{ left: 0, top: 140 }} />
			<HomeHeroDots className={classes.dots} style={{ right: 0, top: 60 }} />

			<div className={classes.inner}>
				<Title className={classes.title}>
					Platform{" "}
					<Text
						component="span"
						className={classes.highlight}
						variant="gradient"
						gradient={{ from: "blue", to: "cyan" }}
						inherit
					>
						AI
					</Text>{" "}
					untuk pertanian
				</Title>

				<Container p={0} size={600}>
					<Text size="lg" color="dimmed" className={classes.description}>
						<b>AgriPen</b> merupakan pengembangan dari aplikasi{" "}
						<b>PadiScanner</b> yang berfungsi untuk memperluas ruang lingkup
						pertanian cerdas berbasis <i>artificial intelligence</i>.
					</Text>
				</Container>

				<div className={classes.controls}>
					<Button
						size="lg"
						className={classes.control}
						rightIcon={<IconBolt color={theme.colors.yellow[4]} />}
						variant="gradient"
						gradient={{ from: "blue", to: "teal", deg: 105 }}
						onClick={() => navigate("/app")}
					>
						AgriPen
					</Button>
				</div>
			</div>
		</Container>
	);
}

const useStyles = createStyles((theme) => ({
	wrapper: {
		position: "relative",
		paddingTop: rem(120),
		paddingBottom: rem(80),

		[theme.fn.smallerThan("sm")]: {
			paddingTop: rem(80),
			paddingBottom: rem(60),
		},
	},

	inner: {
		position: "relative",
		zIndex: 1,
	},

	dots: {
		position: "absolute",
		color:
			theme.colorScheme === "dark"
				? theme.colors.dark[5]
				: theme.colors.gray[1],

		[theme.fn.smallerThan("sm")]: {
			display: "none",
		},
	},

	dotsLeft: {
		left: 0,
		top: 0,
	},

	title: {
		textAlign: "center",
		fontWeight: 800,
		fontSize: rem(40),
		letterSpacing: -1,
		color: theme.colorScheme === "dark" ? theme.white : theme.black,
		marginBottom: theme.spacing.xs,
		fontFamily: `Greycliff CF, ${theme.fontFamily}`,

		[theme.fn.smallerThan("xs")]: {
			fontSize: rem(28),
			textAlign: "left",
		},
	},

	highlight: {
		color:
			theme.colors[theme.primaryColor][theme.colorScheme === "dark" ? 4 : 6],
	},

	description: {
		textAlign: "center",

		[theme.fn.smallerThan("xs")]: {
			textAlign: "left",
			fontSize: theme.fontSizes.md,
		},
	},

	controls: {
		marginTop: theme.spacing.lg,
		display: "flex",
		justifyContent: "center",

		[theme.fn.smallerThan("xs")]: {
			flexDirection: "column",
		},
	},

	control: {
		"&:not(:first-of-type)": {
			marginLeft: theme.spacing.md,
		},

		[theme.fn.smallerThan("xs")]: {
			height: rem(42),
			fontSize: theme.fontSizes.md,

			"&:not(:first-of-type)": {
				marginTop: theme.spacing.md,
				marginLeft: 0,
			},
		},
	},
}));
