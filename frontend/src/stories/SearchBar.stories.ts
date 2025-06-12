import SearchBar from "../components/SearchBar.vue";
import type { Meta, StoryObj } from "@storybook/vue3";

const meta = {
  title: "Components/SearchBar",
  component: SearchBar,
  tags: ["autodocs"],
} satisfies Meta<typeof SearchBar>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Primary: Story = {};
