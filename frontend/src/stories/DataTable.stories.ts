import DataTable from "@/components/DataTable.vue";
import type { Meta, StoryObj } from "@storybook/vue3";

const meta = {
  title: "Components/DataTable",
  component: DataTable,
  tags: ["autodocs"],
  argTypes: {},
} satisfies Meta<typeof DataTable>;
export default meta;

type Story = StoryObj<typeof meta>;
export const Default: Story = {
  args: {},
};
