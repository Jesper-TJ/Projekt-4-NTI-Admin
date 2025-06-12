import ExampleButton from "@/components/ExampleButton.vue";
import type { Meta, StoryObj } from "@storybook/vue3";
// import { VApp } from "vuetify/components";

//Define the component
const meta = {
  title: "Examples/ExampleButton",
  component: ExampleButton,
  tags: ["autodocs"],
  //argTypes decide how you can modify the components props in storybook
  argTypes: {
    color: {
      control: {
        type: "select",
      },
      options: ["primary", "secondary", "success", "info", "warning", "error"],
    },
    loading: {
      control: {
        type: "boolean",
      },
    },
    content: {
      control: {
        type: "text",
      },
    },
  },
  // // decorators are only needed if the component in question requires a v-app to function
  // // You will need to set the height manually to fit the component
  // decorators: [
  //   (story) => ({
  //     components: { story, VApp },
  //     template: `
  //       <v-app style="height: 50px;">
  //           <story />
  //       </v-app>
  //       `,
  //   }),
  // ],
} satisfies Meta<typeof ExampleButton>;
export default meta;

//Display the component
type Story = StoryObj<typeof meta>;
export const Default: Story = {
  args: {},
};

//Dsplay variations of the component
//These aren't necessary but can be good for large comonents.
export const Primary: Story = {
  args: {
    color: "primary",
  },
};
export const Secondary: Story = {
  args: {
    color: "secondary",
  },
};
export const Loading: Story = {
  args: {
    color: "warning",
    loading: true,
  },
};
