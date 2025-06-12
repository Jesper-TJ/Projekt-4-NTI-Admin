// src/types/BookLoan.ts
export type BookLoanPreview = {
  id: number;
  title: string;
  entryDate: string;
  returnDate: string;
  isActive: boolean;
}

export type BookLoanResponse = {
  loanedBooks: BookLoanPreview[];
  totalBookLoans: number;
  activeLoanedBooks: BookLoanPreview[]; 
  totalBookLoansActive: number;
}