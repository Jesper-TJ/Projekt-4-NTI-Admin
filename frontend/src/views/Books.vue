<script setup lang="ts">
import router from "@/router";
import { useBookStore } from "@/store/BookStore";
import type { IBook } from "@/store/BookStore";

const bookStore = useBookStore();
bookStore.fetchBooks();

const books: IBook[] = bookStore.allBooks;
console.log(books)
</script>

<template>
  <h1>Books</h1>
  <div v-if="books.length > 0">
    <ul>
      <li v-for="(book, index) in books" :key="index" style="margin-bottom: 1rem;">
        <strong>{{ book.title }}</strong> by {{ book.author }}
        <p>{{ book.description }}</p>
        <v-btn @click="
          router.push({
            name: 'Book',
            params: {
              isbn: book.isbn,
            },
          })
          ">
          View Book
        </v-btn>
      </li>
    </ul>
  </div>
  <div v-else>
    <p>No books found</p>
  </div>
</template>

<style scoped></style>
