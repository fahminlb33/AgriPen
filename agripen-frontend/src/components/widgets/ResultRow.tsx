import { Text, Group } from "@mantine/core";

export type ResultRowProps = {
  label: string;
  value: string;
  bold?: boolean;
}

export function ResultRow({ label, value, bold }: ResultRowProps) {
  return (
    <Group position="apart">
      <Text>{label}</Text>
      {bold ? <Text fw={700}>{value}</Text> : <Text>{value}</Text>}
    </Group>
  );
}
