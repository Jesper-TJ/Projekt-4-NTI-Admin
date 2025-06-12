<script setup lang="ts">
import Comments from "./BookComments.vue";
import { IComment } from "./BookComments.vue";

defineProps<{
  id?: number;
  title?: string;
  genres?: string[];
  description?: string;
  author?: string;
  isbn?: string;
  comments?: IComment[];
  books?: {id: string; barCode: number; isAvailable: boolean; status: string;};
}>();
</script>

<template>
  <v-card
    max-width="1100"
    variant="elevated"
    elevation="3"
    class="pa-2 mx-auto"
  >
    <v-row>
      <v-col style="max-width: 60%">
        <v-card-title class="text-wrap">
          {{ title }}
        </v-card-title>

        <v-card-subtitle
          ><p v-for="genre in genres" :key="genre">
            {{ genre }}
          </p></v-card-subtitle
        >

        <v-card-text>
          {{ description }}
        </v-card-text>
        <v-card-subtitle>Author: {{ author }}</v-card-subtitle>
        <v-card-subtitle>Isbn: {{ isbn }}</v-card-subtitle>
        <v-card-actions class="d-flex align-center flex-row">
          <v-btn variant="outlined" prepend-icon="mdi-book-plus-multiple"
            >Borrow</v-btn
          >
        </v-card-actions>
        <v-card-text> </v-card-text>
      </v-col>
      <v-col class="ma-10 align">
        <v-img
          aspect-ratio="1/2"
          width="300"
          height="450"
          cover
          src="https://shorturl.at/cMQR6"
        ></v-img>
      </v-col>
    </v-row>
    <v-divider></v-divider>
    <Comments :comments="comments" />
  </v-card>
</template>
