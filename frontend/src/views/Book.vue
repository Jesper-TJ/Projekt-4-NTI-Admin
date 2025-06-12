<script setup lang="ts">
import { useRoute } from "vue-router";
import { useBookStore } from "@/store/BookStore";
import TitleCard from "@/components/TitleCard.vue";

const bookStore = useBookStore();
bookStore.fetchBooks();

const route = useRoute();
const isbn = route.params.isbn as string;

const book = bookStore.bookByIsbn(isbn);

console.log(book)

</script>

<template>
  <!-- Render TitleCard if bookData is found -->
  <div v-if="book">
    <TitleCard v-bind="book" />
  </div>
  <div v-else>
    <p>Book not found with id {{ isbn }}.</p>
  </div>
</template>

<style scoped></style>
