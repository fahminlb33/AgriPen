import {
	createStyles,
	Text,
	Container,
	ActionIcon,
	Group,
	Image,
	Footer,
} from "@mantine/core";
import { Link } from "react-router-dom";

import {
	PUBLIC_FOOTER_LINKS as links,
	FOOTER_SOCIAL_LINKS as socialLinks,
} from "@/routes/menus";

import LogoPng from "@/assets/img/logo.png";

export function RootFooter() {
	const { classes } = useStyles();

	const groups = links.map((group) => {
		const links = group.links.map((link, index) => (
			<Link key={`${link}-${index}`} className={classes.link} to={link.link}>
				{link.label}
			</Link>
		));

		return (
			<div className={classes.wrapper} key={group.title}>
				<Text className={classes.title}>{group.title}</Text>
				{links}
			</div>
		);
	});

	return (
		<Footer height={267} className={classes.footer}>
			<Container className={classes.inner}>
				<div className={classes.logo}>
					<Image width={48} height={48} src={LogoPng} alt="Logo" />
					<Text size="xs" color="dimmed" className={classes.description}>
						<b>Kodesiana #NodingItuMudah</b>
						<br />
						Platform belajar ngoding, cloud, dan machine learning.
					</Text>
				</div>
				<div className={classes.groups}>{groups}</div>
			</Container>
			<Container className={classes.afterFooter}>
				<Text color="dimmed" size="sm">
					Kodesiana © {new Date().getFullYear()}. Hak Cipta Dilindungi
					Undang-Undang.
				</Text>

				<Group spacing={0} className={classes.social} position="right" noWrap>
					{socialLinks.map((socialLink, index) => (
						<a href={socialLink.link} key={`${socialLink.link}-${index}`}>
							<ActionIcon size="lg">
								<socialLink.icon aria-label={socialLink.ariaLabel} />
							</ActionIcon>
						</a>
					))}
				</Group>
			</Container>
		</Footer>
	);
}

const useStyles = createStyles((theme) => ({
	footer: {
		paddingTop: theme.spacing.xl,
		paddingBottom: theme.spacing.xl,
		backgroundColor:
			theme.colorScheme === "dark"
				? theme.colors.dark[6]
				: theme.colors.gray[0],
		borderTop: `1px solid ${
			theme.colorScheme === "dark" ? theme.colors.dark[5] : theme.colors.gray[2]
		}`,
	},

	logo: {
		maxWidth: 200,

		[theme.fn.smallerThan("sm")]: {
			display: "flex",
			flexDirection: "column",
			alignItems: "center",
		},
	},

	description: {
		marginTop: 5,

		[theme.fn.smallerThan("sm")]: {
			marginTop: theme.spacing.xs,
			textAlign: "center",
		},
	},

	inner: {
		display: "flex",
		justifyContent: "space-between",

		[theme.fn.smallerThan("sm")]: {
			flexDirection: "column",
			alignItems: "center",
		},
	},

	groups: {
		display: "flex",
		flexWrap: "wrap",

		[theme.fn.smallerThan("sm")]: {
			display: "none",
		},
	},

	wrapper: {
		width: 160,
	},

	link: {
		display: "block",
		color:
			theme.colorScheme === "dark"
				? theme.colors.dark[1]
				: theme.colors.gray[6],
		fontSize: theme.fontSizes.sm,
		paddingTop: 3,
		paddingBottom: 3,
		textDecoration: "none",

		"&:hover": {
			textDecoration: "underline",
		},
	},

	title: {
		fontSize: theme.fontSizes.lg,
		fontWeight: 700,
		fontFamily: `Greycliff CF, ${theme.fontFamily}`,
		marginBottom: theme.spacing.xs,
		color: theme.colorScheme === "dark" ? theme.white : theme.black,
	},

	afterFooter: {
		display: "flex",
		justifyContent: "space-between",
		alignItems: "center",
		marginTop: theme.spacing.xl,
		paddingTop: theme.spacing.xl,
		paddingBottom: theme.spacing.xl,
		borderTop: `1px solid ${
			theme.colorScheme === "dark" ? theme.colors.dark[4] : theme.colors.gray[2]
		}`,

		[theme.fn.smallerThan("sm")]: {
			flexDirection: "column",
		},
	},

	social: {
		[theme.fn.smallerThan("sm")]: {
			marginTop: theme.spacing.xs,
		},
	},
}));
