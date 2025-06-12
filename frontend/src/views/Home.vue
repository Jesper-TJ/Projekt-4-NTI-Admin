<script setup lang="ts">
import router from "@/router";

function route(name: string) {
  router.push({
    name: name,
  });
}

</script>

<template>
  <!-- <LogIn /> -->
  <h1>Home</h1>
  <v-btn @click="route('Users')">Users</v-btn>
  <v-btn @click="route('Library')">Books</v-btn>
  <v-btn @click="route('Computers')">Computers</v-btn>
  <v-btn @click="route('Support')">Support</v-btn>
</template>

<style scoped></style>
