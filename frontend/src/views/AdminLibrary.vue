<script setup lang="ts">
import { BookTitlePreview } from "@/types/BookTitle";
import { ref, computed, onMounted } from "vue";
import SearchBar from "@/components/SearchBar.vue";
import { useRouter } from "vue-router";
import { IBook, useBookStore } from "@/store/BookStore";

const bookStore = useBookStore();
const router = useRouter();

const search = ref('');
const isLoadingBooks = ref(true);
const itemsPerPageRef = ref(10);

const libraryBooks = ref<IBook[]>([]);
const bookTitles = computed(() => libraryBooks.value);
const filteredBooks = ref<BookTitlePreview[]>([]);

onMounted(async () => {
  isLoadingBooks.value = true;
  await bookStore.fetchBooks();
  libraryBooks.value = bookStore.allBooks;
  filteredBooks.value = bookTitles.value.map((book) => ({
    title: book.title,
    totalBooks: book.totalBooks,
    availableBooks: book.availableBooks,
  }));
  isLoadingBooks.value = false;
});

function handleSearch(searchTerm: string) {
  filteredBooks.value = bookTitles.value.map((book) => ({
    title: book.title,
    totalBooks: book.totalBooks,
    availableBooks: book.availableBooks,
  })).filter((book) =>
    book.title.toLowerCase().includes(searchTerm.toLowerCase())
  );
}

const dialog = ref(false);
const selectedItem = ref<{ title: string; availableBooks: number; totalBooks: number } | null>(null);

function openConfirmDialog(item: { title: string; availableBooks: number; totalBooks: number }) {
  selectedItem.value = item;
  dialog.value = true;
}

function confirmRemove() {
  if (selectedItem.value) {
    filteredBooks.value = filteredBooks.value.filter(
      (book) => book.title !== selectedItem.value!.title
    );
    dialog.value = false;
    selectedItem.value = null;
  }
}

function navigateToLoans() {
  router.push({ name: "Loans" });
}

const headers = [
  { title: "Book Name", key: "title", sortable: false },
  { title: "Available Books", key: "availableBooks", sortable: false },
  { title: "Total Books", key: "totalBooks", sortable: false },
  { title: "Edit", key: "edit", sortable: false },
  { title: "Remove", key: "remove", sortable: false },
];



const editBookDialog = ref(false);
const barcodebooklist = ref();
const editingBARCODE = ref(false)
const selectedBarcode = ref();

function openEdit(item: { title: string; availableBooks: number; totalBooks: number }){
  barcodebooklist.value = bookTitles.value.filter(
    (book) => book.title === item.title
  )
  console.log(barcodebooklist.value)
  editBookDialog.value = true
}

function selectBarcode(event: { target: { value: any; }; }) {
  selectedBarcode.value = event.target.value; // Save the clicked barcode
  editingBARCODE.value = true; // Set editing state
  console.log("Selected Barcode:", selectedBarcode.value); // For debugging
}

const barcodeInput = ref()
const statusInput = ref()
function addItem() {
      console.log('Barcode:', barcodeInput.value);
      console.log('Status:', statusInput.value);
      // Your logic for adding the item
}



// Reactive state for the dialog and form
const addBookDialog = ref(false);
const isFormValid = ref(false);

const newBook = ref({
  title: "",
  author: "",
  description: ""
});

// Function to open the Add Book dialog
function openAddBookDialog() {
  addBookDialog.value = true;
}

// Function to close the Add Book dialog
function closeAddBookDialog() {
  addBookDialog.value = false;
  resetAddBookForm();
}

// Function to reset the form
function resetAddBookForm() {
  newBook.value = {
    title: "",
    author: "",
    description: ""
  };
}

// Function to handle form submission
function submitAddBookForm() {
  if (isFormValid.value) {
    console.log("New book added:", newBook.value);

    closeAddBookDialog();
  }
}
</script>


<template>
  <div class="library-container">
    <v-card class="header" elevation="10">
      <div class="header-content">
        <v-btn
          class="add-books-button"
          elevation="2"
          @click="openAddBookDialog"
        >
          Add title
        </v-btn>
        <v-dialog v-model="addBookDialog" max-width="500px">
          <v-card class="dialog-card">
            <v-card-title class="headline dialog-title">Add New Book</v-card-title>
            <v-card-text class="dialog-text">
              <v-form ref="addBookForm" v-model="isFormValid">
                <v-text-field
                  label="Book Title"
                  v-model="newBook.title"
                  required
                  outlined
                ></v-text-field>
                <v-text-field
                  label="Author"
                  v-model="newBook.author"
                  required
                  outlined
                ></v-text-field>
                <v-text-field
                  label="Description"
                  v-model="newBook.description"
                  required
                  outlined
                ></v-text-field>
              </v-form>
            </v-card-text>
            <v-card-actions class="dialog-actions">
              <v-spacer></v-spacer>
              <v-btn
                color="blue-grey"
                class="dialog-cancel-button"
                @click="closeAddBookDialog"
              >
                Cancel
              </v-btn>
              <v-btn
                color="green"
                class="dialog-save-button"
                :disabled="!isFormValid"
                @click="submitAddBookForm"
              >
                Add
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <h2 class="title">Library Titles</h2>
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
        :items-length="libraryBooks.length"
        :items="filteredBooks"
        :loading="isLoadingBooks"
        item-value="name"
        loading-text="Loading titles ..."
        no-data-text="No available books"
        class="data-table"
      >
      <template #item.edit="{ item }">
        <v-btn color="green" small @click="openEdit(item)">
          Edit
        </v-btn>
      </template>
      <template #item.remove="{ item }">
        <v-btn color="red" small @click="openConfirmDialog(item)">
          Remove
        </v-btn>
      </template>

    </v-data-table-server>

    </v-card>

    <v-dialog v-model="dialog" max-width="400px">
  <v-card class="dialog-card">
    <v-card-title class="headline dialog-title">Confirm Removal</v-card-title>
    <v-card-text class="dialog-text">
      Are you sure you want to remove <strong>{{ selectedItem?.title }}</strong>?
    </v-card-text>
    <v-card-actions class="dialog-actions">
      <v-spacer></v-spacer>
      <v-btn
        color="blue-grey"
        class="dialog-cancel-button"
        @click="dialog = false"
      >
        Cancel
      </v-btn>
      <v-btn
        class="dialog-remove-button"
        @click="confirmRemove"
      >
        Remove
      </v-btn>
    </v-card-actions>
  </v-card>
</v-dialog>

<v-dialog v-model="editBookDialog" max-width="600px">
  <template v-slot:default>
    <v-card class="dialog-card">
      <v-card-title class="dialog-card-title">Edit Book: ISBN {{ barcodebooklist[0].isbn }}</v-card-title>
      <v-card-text class="dialog-card-text">
        <div class="dialog-content">


          <!-- Left Side: Datalist for Barcodes -->
          <div class="scrollable-list">
            <h3 style="color: #00adb5;">Barcodes:</h3>
            <v-text-field
              label="Search Barcode"
              v-model="selectedBarcode"
              :autocomplete="'off'"
              :list="'barcodeList'"
              dense
              outlined
              @keyup.enter.prevent="selectBarcode($event)"
            ></v-text-field>
            <datalist id="barcodeList">
              <template v-for="(book, index) in barcodebooklist" :key="index">
                <option
                  v-for="(barcode, bIndex) in book.books"
                  :key="bIndex"
                  :value="barcode.barCode"
                >
                  {{ barcode.barCode }}
                </option>
              </template>
            </datalist>
          </div>

          <!-- Right Side: Inputs and Remove Button -->
          <div class="dialog-right">
            <div v-if="!editingBARCODE">
              <v-text-field
                label="Barcode"
                v-model="barcodeInput"
                outlined
                dense
              ></v-text-field>
              <v-select
                label="Status"
                v-model="statusInput"
                :items="['Fine', 'Broken', 'Lost']"
                outlined
                dense
              ></v-select>
              <v-btn
                class="dialog-add-button"
                block
                @click="addItem"
              >
                Add
              </v-btn>
            </div>

            <div v-else-if="editBookDialog && editingBARCODE">
              <h4>Selected Barcode: {{ selectedBarcode }}</h4>
              <v-select
                label="Status"
                v-model="statusInput"
                :items="['Fine', 'Broken', 'Lost']"
                outlined
                dense
              ></v-select>
              <v-btn
                class="dialog-add-button"
                block
                @click="addItem"
              >
                Update
              </v-btn>
              <v-btn
                color="red"
                style="margin-top: 20px"
                @click=""
              >
                Remove
              </v-btn>
            </div>
          </div>
        </div>
      </v-card-text>
      <v-card-actions class="dialog-card-actions">
        <v-btn class="dialog-btn" @click="editBookDialog = false, editingBARCODE = false">Close</v-btn>
      </v-card-actions>
    </v-card>
  </template>
</v-dialog>






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

.add-books-button{
  position: absolute;
  top: 0;
  left: 10px; /* Slight padding from the right edge */
  background-color: #00adb5;
  color: white;
  font-weight: bold;
}
.add-books-button:hover {
  background-color: #007f8c; /* Darker teal on hover */
}

.scrollable-list {
  flex: 1; /* Take up remaining space */
  background-color: #2d3540;
  border-radius: 5px;
  max-height: 200px;
  overflow-y: auto;
  padding: 10px;
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

/* Dialog box */

.dialog-content {
  display: flex;
  gap: 20px; /* Space between left and right sections */
}

.dialog-card {
  background-color: #222831;
  color: white;
  border-radius: 12px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.7);
}

.dialog-title {
  color: #00adb5; /* Match your teal accent */
  font-weight: bold;
  font-size: 1.5rem;
}

.dialog-card-title {
  color: #00adb5; /* Match your teal accent */
  font-weight: bold;
  font-size: 1.5rem;
}

.dialog-text {
  color: #eeeeee; /* Light text color for contrast */
  font-size: 1rem;
  line-height: 1.5;
  margin-left: 5px;
}

.dialog-actions {
  padding: 0 16px 16px; /* Consistent padding for dialog actions */
}

.dialog-cancel-button {
  background-color: #393e46;
  color: #eeeeee;
  font-weight: bold;
}

.dialog-cancel-button:hover {
  background-color: #4a4f58; /* Slightly lighter for hover effect */
}

.dialog-right {
  flex: 1; /* Take up remaining space */
  display: flex;
  flex-direction: column;
  justify-content: space-between; /* Push the remove button to the bottom */
  gap: 10px; /* Space between inputs and button */
}

.dialog-add-button {
  align-self: flex-end; /* Align to the right */
  background-color: #00adb5; /* Red for remove button */
  color: white;
  font-weight: bold;
}

.dialog-add-button:hover {
  background-color: #00adb5; /* Slightly lighter red on hover */
}

.barcodes:hover{

  background-color: #4a4f58;
  border-radius: 5px;
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
