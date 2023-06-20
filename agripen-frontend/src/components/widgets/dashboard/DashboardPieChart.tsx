import { Divider, Paper, Title, Text } from "@mantine/core";
import {
	Cell,
	Legend,
	Pie,
	PieChart,
	ResponsiveContainer,
	Tooltip,
} from "recharts";

export interface DashboardPieChartProps {
	title: string;
	data: unknown[];
}

const COLORS = ["#0088FE", "#00C49F", "#FFBB28", "#FF8042"];

const RADIAN = Math.PI / 180;
const renderCustomizedLabel = ({
	cx,
	cy,
	midAngle,
	innerRadius,
	outerRadius,
	percent,
}: {
	cx: number;
	cy: number;
	midAngle: number;
	innerRadius: number;
	outerRadius: number;
	percent: number;
	index: number;
}) => {
	const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
	const x = cx + radius * Math.cos(-midAngle * RADIAN);
	const y = cy + radius * Math.sin(-midAngle * RADIAN);

	return (
		<text
			x={x}
			y={y}
			fill="white"
			textAnchor={x > cx ? "start" : "end"}
			dominantBaseline="central"
		>
			{`${(percent * 100).toFixed(0)}%`}
		</text>
	);
};

export function DashboardPieChart({ title, data }: DashboardPieChartProps) {
	return (
		<Paper withBorder radius="md">
			<Title size="xs" color="dimmed" pt="md" pl="lg">
				{title}
			</Title>
			<Divider mt="md" />

			<div style={{ width: "100%", height: 300, paddingBottom: "20px" }}>
				{!data || data.length === 0 ? (
					<Text align="center" pt="140px">
						Tidak ada data.
					</Text>
				) : (
					<ResponsiveContainer>
						<PieChart data={data}>
							<Legend />
							<Tooltip />
							<Pie
								dataKey="value"
								nameKey="name"
								data={data}
								labelLine={false}
								label={renderCustomizedLabel}
							>
								{data.map((_, index) => (
									<Cell
										key={`cell-${index}`}
										fill={COLORS[index % COLORS.length]}
									/>
								))}
							</Pie>
						</PieChart>
					</ResponsiveContainer>
				)}
			</div>
		</Paper>
	);
}
