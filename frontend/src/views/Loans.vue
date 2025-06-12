<script setup lang="ts">
import Loans from "@/components/Loans.vue";
import { ref, Ref, onMounted } from "vue";
import { DataTableFetchOpts } from "@/types/VuetifyTypes";
import { IBook, useBookStore, BookLoanPreview } from "@/store/BookStore";

// src/types/BookLoan.ts

export type BookLoanResponse = {
  loanedBooks: BookLoanPreview[];
  totalBookLoans: number;
  activeLoanedBooks: BookLoanPreview[]; 
  totalBookLoansActive: number;
}

const bookStore = useBookStore();
const isLoading = ref(true);
const userId = ref(1);

bookStore.fetchLoanedBooks(userId.value).then(() => {
  isLoading.value = false;
  // activeLoanedBooks.value = bookStore.allLoanedBooks;
  loanedBooks.value = bookStore.allLoanedBooks.filter(book => book.status === 'Returned');
  activeLoanedBooks.value = bookStore.allLoanedBooks.filter(book => book.status === 'Borrowed' || book.status === 'Overdue');
});

const books = ref<IBook[]>([]);
const activeLoanedBooks = ref<BookLoanPreview[]>([]);
const totalBookLoans = ref(0);
const totalBookLoansActive = ref(0);

// Hard coded userId

const date = ref(new Date());

const loanedBooks = ref<BookLoanPreview[]>([]);
</script>

<template>
  <Loans
    :active-loaned-books="activeLoanedBooks"
    :loaned-books="loanedBooks"
    :total-book-loans="loanedBooks.length"
    :total-book-loans-active="activeLoanedBooks.length"
    :is-loading="isLoading"
  />
</template>

<style scoped></style>
