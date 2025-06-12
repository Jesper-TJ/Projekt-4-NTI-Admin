import TitleCard from "@/components/TitleCard.vue";
import type { Meta, StoryObj } from "@storybook/vue3";

const meta = {
  title: "Components/TitleCard",
  component: TitleCard,
  tags: ["autodocs"],
  argTypes: {
    title: {
      control: {
        type: "text",
      },
    },
  },
} satisfies Meta<typeof TitleCard>;

export default meta;

type Story = StoryObj<typeof meta>;
export const Default: Story = {};

export const Example: Story = {
  args: {
    title: "The Hitchhiker's Guide to the Galaxy",
    description: `The Hitchhiker's Guide to the Galaxy is the first book in the
        Hitchhiker's Guide to the Galaxy comedy science fiction "trilogy of
        five books" by Douglas Adams, with a sixth book written by Eoin
        Colfer. The novel is an adaptation of the first four parts of Adams's
        radio series of the same name, centering on the adventures of the only
        man to survive the destruction of Earth; while roaming outer space, he
        comes to learn the truth behind Earth's existence. The novel was first
        published in London on 12 October 1979. It sold 250,000 copies in the
        first three months. The namesake of the novel is The Hitchhiker's
        Guide to the Galaxy, a fictional guide book for hitchhikers (inspired
        by the Hitch-hiker's Guide to Europe) written in the form of an
        encyclopaedia.`,
    author: "Douglas Adams",
    isbn: "1337",
    genres: ["science fiction", "comedy"],
    comments: [
      {
        userName: "Jason",
        rating: "1/5",
        commentText: "I hate it",
      },
      {
        userName: "Mason",
        rating: "2/5",
        commentText: "It aight",
      },
      {
        userName: "Chason",
        rating: "6/5",
        commentText: "I quite enjoy reading this from time to time",
      },
    ],
  },
};
