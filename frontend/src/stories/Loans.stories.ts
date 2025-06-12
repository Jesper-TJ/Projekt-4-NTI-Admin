import Loans from "@/components/Loans.vue";
import type { Meta, StoryObj } from "@storybook/vue3";

const meta = {
  title: "Components/Loans",
  component: Loans,
  tags: ["autodocs"],
  argTypes: {},
} satisfies Meta<typeof Loans>;
export default meta;

type Story = StoryObj<typeof meta>;
export const Default: Story = {
  args: {
    loanedBooks: [], // Assuming an empty array is a valid default
    activeLoanedBooks: [], // Same here
    totalBookLoans: 0, // Default number
    totalBookLoansActive: 0, // Default number
    isLoading: false, // Example default value
  },
};
