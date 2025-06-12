<script setup lang="ts">
import { ref } from 'vue';

// src/types/BookLoan.ts
export type BookLoanPreview = {
  id: number;
  title: string;
  returnAt: string;
  returnDate: string;
  createdAt: string;
  status: string;
}

export type BookLoanResponse = {
  loanedBooks: BookLoanPreview[];
  totalBookLoans: number;
  activeLoanedBooks: BookLoanPreview[]; 
  totalBookLoansActive: number;
}

interface Props {
  loanedBooks: BookLoanPreview[];
  activeLoanedBooks: BookLoanPreview[];
  totalBookLoans: number;
  totalBookLoansActive: number;
  isLoading: boolean;
}

defineProps<Props>();

const itemsPerPage = ref(10);
const filter = ref('active');

const headers = [
  { title: "Title", key: "title", sortable: false, width: "35%" },
  { title: "Borrowed Date", key: "createdAt", sortable: false, width: "25%" },
  { title: "Return Date", key: "returnDate", sortable: false, width: "25%" },
  { 
    title: "Status", 
    key: "status", 
    sortable: false,
    render: (value: 'Returned' | 'Borrowed' | 'Overdue') => {
      const statusClass = {
        'Returned': 'returned-status',
        'Borrowed': 'borrowed-status',
        'Overdue': 'overdue-status'
      }[value];
      return `<span class="status-badge ${statusClass}">${value}</span>`;
    },
    width: "15%"
  },
];
</script>

<template>
  <div class="loans-container">
    <v-card class="header" elevation="10">
      <div class="header-content">
        <h2 class="title">Loans</h2>
      </div>
      
      <v-card-actions class="data-table-card" elevation="10">
        <v-switch
          v-model="filter"
          class="toggle-switch"
          inset
          color="#00adb5"
          :true-value="'all'"
          :false-value="'active'"
          label="Show Previous Loans"
        ></v-switch>
      </v-card-actions>

      <v-data-table-server
        v-model:items-per-page="itemsPerPage"
        :headers="headers"
        :items="filter === 'active' ? activeLoanedBooks : loanedBooks"
        :items-length="filter === 'active' ? totalBookLoansActive : totalBookLoans"
        :loading="isLoading"
        class="data-table"
      >
        <template #no-data>
          <div class="no-data">Failed to fetch loans (|||❛︵❛.)</div>
        </template>
        <template #loading>
          <div class="loading">Loading loans...</div>
        </template>
        <template #item.status="{ item }">
          <span class="status-badge" :class="{
            'returned-status': item.status === 'Returned',
            'borrowed-status': item.status === 'Borrowed',
            'overdue-status': item.status === 'Overdue'
          }">
            {{ item.status }}
          </span>
        </template>
      </v-data-table-server>
    </v-card>
  </div>
</template>

<style scoped>
.status-badge {
  padding: 4px 8px;
  border-radius: 4px;
  font-weight: 500;
  color: white;
  display: inline-block;
}

.returned-status {
  background-color: rgba(76, 175, 80, 0.9); /* Green */
}

.borrowed-status {
  background-color: rgba(255, 152, 0, 0.9); /* Orange */
}

.overdue-status {
  background-color: rgba(244, 67, 54, 0.9); /* Red */
}

/********* General Layout *********/
.loans-container {
  padding-left: 40px;
  padding-right: 40px;
}

/********* Header Styling *********/
.header {
  background-color: #222831;
  padding: 20px;
  border-radius: 12px;
  margin-bottom: 20px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.7);
}

.header-content {
  text-align: center;
}

.title {
  font-size: 1.8rem;
  font-weight: bold;
  color: #00adb5;
  margin: 0;
}


/********* Toggle Switch Styling *********/
.toggle-switch {
  background-color: #222831;
  color: white;
}

.toggle-switch .v-input__control {
  background-color: #222831;
  color: white;
  border-radius: 12px;
}

.toggle-switch .v-input--switch__track {
  background-color: #00adb5;
}

.toggle-switch .v-input--switch__thumb {
  background-color: #00adb5;
}

/********* Data Table Styling *********/
.data-table-card {
  background-color: #222831;
  border-radius: 12px;
  padding: 20px;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.3);
}

.data-table {
  border-radius: 8px;
  background-color: #222831;
  color: white;
}

:deep(.v-data-table thead) {
  color: #00adb5;
  font-weight: bold;
  font-size: 1.1rem;
}

:deep(.v-data-table tbody tr) {
  color: white;
}

:deep(.v-data-table td) {
  color: white;
}

/********* Custom States *********/
.no-data,
.loading {
  text-align: center;
  color: #bbbbbb;
  font-size: 1.2rem;
  padding: 20px;
}

/********* Hover Effects *********/
:deep(.v-data-table tbody tr:hover) {
  background-color: #393e46;
  cursor: pointer;
}

/********* Responsive Adjustments *********/
@media screen and (max-width: 768px) {
  .toggle-container {
    flex-direction: column;
    align-items: center;
  }
}
</style>
