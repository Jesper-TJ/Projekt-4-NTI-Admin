import ManageCard from "../components/ManageCard.vue";
import type { Story } from "@storybook/vue3";

export default {
  title: "Components/ManageCard",
  component: ManageCard,
  argTypes: {
    imageSrc: {
      control: "text",
      type: {
        name: "string",
        required: true,
      },
    },
    cardTitle: {
      control: "text",
      type: {
        name: "string",
        required: true,
      },
    },
    items: {
      control: "array",
      type: {
        name: "array",
        required: true,
      },
    },
    manageButtonLabel: {
      control: "text",
      type: {
        name: "string",
        required: true,
      },
    },
    updatedStatusButtonLabel: {
      control: "text",
      type: {
        name: "string",
        required: true,
      },
    },
  },
};

const Template: Story<{
  imageSrc: string;
  cardTitle: string;
  items: string[];
  manageButtonLabel: string;
  updatedStatusButtonLabel: string;
}> = (args) => ({
  components: {
    ManageCard,
  },
  setup() {
    return {
      args,
    };
  },
  template: '<manage-card v-bind="args" />',
});

export const ComputerCard = Template.bind({});
ComputerCard.args = {
  imageSrc:
    "https://content.instructables.com/F4N/K84M/JDUKKGUH/F4NK84MJDUKKGUH.jpg?auto=webp&frame=1&width=1024&height=1024&fit=bounds&md=9b3dc16f37f12c8ec5aaaca97c447292",
  cardTitle: "Computer",
  items: ["Computer Status", "Service Overview", "Service Request"],
  manageButtonLabel: "Manage",
  updatedStatusButtonLabel: "Updated Status",
};

export const LibraryCard = Template.bind({});
LibraryCard.args = {
  imageSrc:
    "https://tutoringsolutionsgroup.com/wp-content/uploads/2022/06/shutterstock_1937995444-750x500.jpg",
  cardTitle: "Library",
  items: ["Library books overview", "Course Books"],
  manageButtonLabel: "View",
  updatedStatusButtonLabel: "Overdue Books",
};

export const NoImageCard = Template.bind({});
NoImageCard.args = {
  imageSrc: "",
  cardTitle: "",
  items: [],
  manageButtonLabel: "",
  updatedStatusButtonLabel: "",
};
