export type BookLoanPreview = {
  id: number;
  title: string;
  returnDate: string;
  entryDate: string;
};

export type BookLoanPreviewResponse = {
  BookLoans: BookLoanPreview[];
  totalBookLoans: number;
};
