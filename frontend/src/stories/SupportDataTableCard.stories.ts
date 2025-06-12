import SupportDataTableCard from "@/components/SupportDataTableCard.vue";
import type { Meta, StoryObj } from "@storybook/vue3";

const meta = {
  title: "Components/Support/Support Datatable Card",
  component: SupportDataTableCard,
  tags: ["autodocs"],
} satisfies Meta<typeof SupportDataTableCard>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Admin: Story = {
  args: {
    isOpen: false,
    item: {
      id: "1",
      name: "Nicolai Zackrisson",
      title: "Broken Ipad",
      description: "I smashed my ipad with a hammer!",
      createdAt: "11:00:28",
      status: "Request",
    },
    user: {
      name: "Nicolai Zackrisson",
      admin: true,
    },
  },
};

export const User: Story = {
  args: {
    isOpen: false,
    item: {
      id: "1",
      name: "Nicolai Zackrisson",
      title: "Broken Ipad",
      description: "I smashed my ipad with a hammer!",
      createdAt: "11:00:28",
      status: "Request",
    },

    user: {
      name: "Nicolai Zackrisson",
      admin: false,
    },
  },
};
