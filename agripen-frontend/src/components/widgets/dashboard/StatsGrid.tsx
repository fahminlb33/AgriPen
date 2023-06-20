import {
	createStyles,
	Group,
	Paper,
	SimpleGrid,
	Text,
	rem,
	Stack,
} from "@mantine/core";

export interface MetricItem {
	title: string;
	value: string;
	icon: any;
}

export interface StatsGridProps {
	data: MetricItem[];
}

export function StatsGrid({ data }: StatsGridProps) {
	const { classes } = useStyles();

	return (
		<div>
			<SimpleGrid
				cols={4}
				breakpoints={[
					{ maxWidth: "md", cols: 2 },
					{ maxWidth: "xs", cols: 1 },
				]}
			>
				{data.map((stat, i) => {
					return (
						<Paper
							withBorder
							p="md"
							radius="md"
							key={`stats-metrics-${stat.title.toLowerCase()}-${i}`}
						>
							<Group position="left">
								<stat.icon className={classes.icon} size="3rem" />
								<Stack spacing={0} mb={5}>
									<Text size="xs" color="dimmed" className={classes.title}>
										{stat.title}
									</Text>
									<Text className={classes.value}>{stat.value}</Text>
								</Stack>
							</Group>
						</Paper>
					);
				})}
			</SimpleGrid>
		</div>
	);
}

const useStyles = createStyles((theme) => ({
	value: {
		fontSize: rem(24),
		fontWeight: 700,
		lineHeight: 1,
	},

	diff: {
		lineHeight: 1,
		display: "flex",
		alignItems: "center",
	},

	icon: {
		color:
			theme.colorScheme === "dark"
				? theme.colors.dark[3]
				: theme.colors.gray[6],
	},

	title: {
		fontWeight: 700,
		textTransform: "uppercase",
	},
}));
