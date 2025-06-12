<script setup lang="ts">
import { BookTitlePreview } from "@/types/BookTitle";
import { ref, computed, onMounted } from "vue";
import SearchBar from "@/components/SearchBar.vue";
import { useRouter } from "vue-router";
import { IBook, useBookStore } from "@/store/BookStore";

const bookStore = useBookStore();
const router = useRouter();

const search = ref('')

// Reactive state
const isLoadingBooks = ref(true);
const itemsPerPageRef = ref(10);

// Computed property for reactive store data
const libraryBooks = ref<IBook[]>([])
const bookTitles = computed(() => libraryBooks.value);
const totalBookTitles = computed(() => libraryBooks.value.length)
const filteredBooks = ref<BookTitlePreview[]>([]);

// Fetch data when the component is mounted
onMounted(async () => {
  isLoadingBooks.value = true;
  await bookStore.fetchBooks();
  libraryBooks.value = bookStore.allBooks
  filteredBooks.value = bookTitles.value.map((booktitle: IBook) => {
    return{
      title: booktitle.title,
      totalBooks: booktitle.totalBooks,
      availableBooks: booktitle.availableBooks
    }
  }) as BookTitlePreview[];
  isLoadingBooks.value = false;
});



// Navigation function
function navigateToBookDetails(book: {id: number}) {
  router.push({ name: "BookDetailsView", params: { id: book.id } });
}

// Row click handler
function onRowClick(event: MouseEvent) {
  const target = event.target as HTMLElement | null;
  const rowElement = target?.closest("tr");
  const rowIndex = rowElement ? rowElement.rowIndex - 1 : -1; // Adjust for headers
  const book = bookTitles.value[rowIndex];
  console.log(book)
  if (book) {
    navigateToBookDetails({
      id: book.id,
    });
    
  } else {
    console.error("Row data not found.");
  }
}

// Search functionality
function handleSearch(searchTerm: string) {
  filteredBooks.value = bookTitles.value.map((booktitle: IBook) => {
    return{
      title: booktitle.title,
      totalBooks: booktitle.totalBooks,
      availableBooks: booktitle.availableBooks
    }
  }).filter((bookTitle) =>
    bookTitle.title.toLowerCase().includes(searchTerm.toLowerCase())
  );
}

// Headers for data table
const headers = [
  { title: "Book Name", key: "title", sortable: false },
  { title: "Available Books", key: "availableBooks", sortable: false },
  { title: "Total Books", key: "totalBooks", sortable: false },
];

// Function to navigate to loaned books page
function navigateToLoans() {
  router.push({ name: "Loans" });
}
</script>

<template>
  <div class="library-container">
    <v-card class="header" elevation="10">
      <div class="header-content">
        <h2 class="title">Library Books</h2>
        <v-btn
          class="loaned-books-button"
          elevation="2"
          @click="navigateToLoans"
        >
          View Loaned Books
        </v-btn>
      </div>
      <div class="search-container">
        <SearchBar class="search-bar" @search="handleSearch" />
      </div>

      <v-data-table-server
        v-model:items-per-page="itemsPerPageRef"
        :headers="headers"
        :items-length="totalBookTitles"
        :items="filteredBooks"
        :loading="isLoadingBooks"
        item-value="name"
        loading-text="Loading titles ..."
        no-data-text="No available books"
        class="data-table"
        @click:row="onRowClick"
      ></v-data-table-server>
    </v-card>
  </div>
</template>


<style scoped>
/* Positioning adjustments for header content */
.header-content {
  position: relative;
  text-align: center; /* Center align the title */
  margin-bottom: 20px;
}

.loaned-books-button {
  position: absolute;
  top: 0;
  right: 10px; /* Slight padding from the right edge */
  background-color: #00adb5;
  color: white;
  font-weight: bold;
}
.loaned-books-button:hover {
  background-color: #007f8c; /* Darker teal on hover */
}

/* Header card */
.header {
  background-color: #222831;
  margin: 0 auto; /* Center the header */
  padding: 20px 10px 10px 10px; /* Added 20px padding at the top and 10px padding on other sides */
  border-radius: 12px; /* Rounded corners */
  margin-bottom: 20px; /* Margin at the bottom */
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.7);
}

/* Overall container */
.library-container {
  padding-left: 40px;
  padding-right: 40px;
}

/* Page title */
.title {
  font-size: 1.8rem;
  font-weight: bold;
  color: #00adb5; /* Teal accent */
  margin: 0; /* Remove default margin */
  line-height: 1.5rem;
}

/* Search bar container */
.search-container {
  display: flex;
  justify-content: center;
  margin-bottom: 20px;
}

/* Search bar */
.search-bar {
  width: 50%;
  max-width: 400px;
  border-radius: 8px;
  box-shadow: 0px 2px 6px rgba(0, 0, 0, 0.2);
  background-color: #393e46;
  color: #eeeeee;
}

/* Data table container */
.data-table {
  border-radius: 8px;
  background-color: #222831;
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.3);
  color: white;
}

/* Use :deep() to target the Vuetify internal data table elements */
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

/* Loading state text */
:deep(.v-data-table .v-data-table__progress) {
  color: #eeeeee;
}

/* Hover effect for rows */
:deep(.v-data-table tbody tr:hover) {
  background-color: #393e46;
  cursor: pointer;
}

/* Responsive adjustments */
@media screen and (max-width: 768px) {
  .search-bar {
    width: 80%;
  }
}
</style>
