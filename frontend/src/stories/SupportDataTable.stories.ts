import SupportDataTable from "@/components/SupportDataTable.vue";
import type { Meta, StoryObj } from "@storybook/vue3";

const meta = {
  title: "Components/Support/Support Datatable",
  component: SupportDataTable,
  tags: ["autodocs"],
} satisfies Meta<typeof SupportDataTable>;

export default meta;

type Story = StoryObj<typeof meta>;

export const Admin: Story = {
  args: {
    isOpen: false,
    items: [
      {
        id: "1",
        name: "Nicolai Zackrisson",
        title: "Broken Ipad",
        description: "I smashed my ipad with a hammer!",
        createdAt: "11:00:28",
        status: "Request",
      },
      {
        id: "2",
        name: "Linus Boström",
        title: "Computer not working",
        description: "My computer is not working!",
        createdAt: "21:10:28",
        status: "Accepted",
      },
    ],

    user: { name: "Nicolai Zackrisson", admin: true },
  },
};

export const User: Story = {
  args: {
    isOpen: false,
    items: [
      {
        id: "1",
        name: "Nicolai Zackrisson",
        title: "Broken Ipad",
        description: "I smashed my ipad with a hammer!",
        createdAt: "11:00:28",
        status: "Request",
      },
      {
        id: "2",
        name: "Linus Boström",
        title: "Computer not working",
        description: "My computer is not working!",
        createdAt: "21:10:28",
        status: "Accepted",
      },
    ],

    user: { name: "Nicolai Zackrisson", admin: false },
  },
};
