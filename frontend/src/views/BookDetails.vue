<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useRoute } from "vue-router";
import BookInfoCard from "@/components/BookInfoCard.vue";
import { IBook, IComment, useBookStore } from "@/store/BookStore";
import BookComments from "@/components/BookComments.vue";

const bookStore = useBookStore();
const route = useRoute();
const bookid = ref(route.params.id as string);
const result = ref(0)
const bookInfo = ref();
const color = ref("black");

// Function to fetch the book data
const fetchBookData = async () => {
  await bookStore.fetchBooks();
  bookInfo.value = bookStore.chosenBookInfo(parseInt(bookid.value));
};

// Handler for the refresh event
const handleRefresh = () => {
  fetchBookData();
};

// Fetch data on component mount
onMounted(fetchBookData);
</script>

<template>
  <BookInfoCard 
    :bookDetails="bookInfo" 
    :color="color" 
    :result="result" 
    :key="bookInfo"
  />
  <BookComments 
    v-if="bookInfo" 
    :bookReviews="bookInfo" 
    :bookid="Number(bookid)" 
    :key="bookInfo"
    @refresh="handleRefresh"
  />
</template>