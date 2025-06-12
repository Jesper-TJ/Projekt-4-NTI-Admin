import ComputersInfoCard from "@/components/ComputerCard.vue";
import type { Meta, StoryObj } from "@storybook/vue3";
import { VApp } from "vuetify/components";

const meta = {
  title: "Components/ComputersInfoCard",
  component: ComputersInfoCard,
  tags: ["autodocs"],
  decorators: [
    (story) => ({
      components: { story, VApp },
      template: `
        <v-app style="height: 50px;">
            <story />
        </v-app>
        `,
    }),
  ],
} satisfies Meta<typeof ComputersInfoCard>;
export default meta;

type Story = StoryObj<typeof meta>;

export const example: Story = {
  args: {
    ownerName: "NameLastname",
    computerName: "MAC AIR",
    ownerRole: "Pro crastination",
    issue: "!",
  },
};
export const example2: Story = {
  args: {
    ownerName: "NAME2",
    computerName: "MACBOOK PRO",
    ownerRole: "Pro crastination",
    issue: "!",
  },
};

