import NavBar from "@/components/NavBar.vue";
import type { Meta, StoryObj } from "@storybook/vue3";
import { VApp } from "vuetify/components";

const meta = {
  title: "Components/NavBar",
  component: NavBar,
  tags: ["autodocs"],
  argTypes: {
    signedIn: {
      control: {
        type: "boolean",
      },
    },
  },
  decorators: [
    (story) => ({
      components: {
        story,
        VApp,
      },
      template: `    
        <v-app style="height: 50px;">
            <story />
        </v-app>
        `,
    }),
  ],
} satisfies Meta<typeof NavBar>;
export default meta;

type Story = StoryObj<typeof meta>;
export const Default: Story = {
  args: {},
};

export const SignedIn: Story = {
  args: {
    signedIn: true,
  },
};
export const SignedOut: Story = {
  args: {
    signedIn: false,
  },
};
