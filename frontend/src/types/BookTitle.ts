export type BookTitlePreview = {
  title: string;
  availableBooks: number;
  totalBooks: number;
};

export type BookTitlePreviewResponse = {
  bookTitles: BookTitlePreview[];
  totalBookTitles: number;
};
