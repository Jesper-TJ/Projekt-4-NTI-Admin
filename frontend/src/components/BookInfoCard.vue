<template>
  <v-row justify="center">
    <v-col cols="12" md="8" lg="6">
      <v-card class="book-card">
        <v-card-item>
          <div class="book-details">
            <div v-if="bookDetails" class="details-container">
                <h2 class="text-h5 title">{{ bookDetails.title }}</h2>
                <div>
                  <v-icon
                    v-for="(icon, index) in starIcons"
                    :key="index"
                    :size="28"
                    color="yellow"
                    :icon="icon"
                  ></v-icon>
                  {{ bookDetails.rating.toFixed(1) }}/5
                </div>
              <h3 class="text-subtitle-1 author">Author: {{ bookDetails.author }}</h3>
              <p class="text-body-1 description">
                {{ bookDetails.description }}
              </p>
              <v-divider class="my-4" />
              <p class="availability">
                <strong>Available:</strong> {{ bookDetails.availableBooks }} / {{ bookDetails.totalBooks }}
              </p>
            </div>
            <div v-else>
              <p v-if="bookDetails === null" class="text-error">
                Failed to load book details. Please try again later.
              </p>
              <p v-else class="loading-message">
                Loading book details...
              </p>
            </div>
          </div>
        </v-card-item>
        <v-card-actions v-if="bookDetails" class="actions">
          <v-btn
            class="action-button"
            v-if="bookDetails.availableBooks > 0"
            prepend-icon="mdi-book-plus-multiple"
            @click="borrowBook"
          >
            Borrow Book
          </v-btn>
          <p v-else class="not-available">

            Not available <v-icon icon="mdi-store-off-outline"></v-icon>
          </p>
        </v-card-actions>
      </v-card>
    </v-col>
  </v-row>
</template>



<script setup lang="ts">
import { computed} from "vue";
import { IBook } from "@/store/BookStore";
import { useBookStore } from "@/store/BookStore";

const bookStore = useBookStore();

const props = defineProps<{
  bookDetails: IBook;
  color: string;
}>();

const starIcons = computed(() => {
const icons = [];
let ratingLeft = props.bookDetails?.rating ?? 0;

for (let i = 0; i < 5; i++) {
  if (ratingLeft >= 1) {
    icons.push("mdi-star"); // Full star
    ratingLeft -= 1;
  } else if (ratingLeft >= 0.5) {
    icons.push("mdi-star-half-full"); // Half star
    ratingLeft -= 0.5;
  } else {
    icons.push("mdi-star-outline"); // Empty star
  }
}

return icons;
});

const borrowBook = async () => {
  if (props.bookDetails) {
    await bookStore.borrowABook(props.bookDetails.id, 1);
  }
};
</script>



<style scoped>
/* General styles */
.book-card {
background-color: #222831; /* Dark gray */
color: #eeeeee; /* Light gray text */
border-radius: 12px;
box-shadow: 0 6px 20px rgba(0, 0, 0, 0.7);
padding: 20px;
overflow: hidden;
transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.book-card:hover {
transform: scale(1.02);
box-shadow: 0px 6px 15px rgba(0, 0, 0, 0.5);
}

/* Title and author */
.title {
font-size: 1.5rem;
font-weight: bold;
color: #00adb5; /* Teal accent */
}

.author {
font-size: 1.2rem;
font-style: italic;
color: #eeeeee;
}

/* Description */
.description {
margin-top: 10px;
line-height: 1.6;
color: #cfcfcf; /* Slightly muted for body text */
}

/* Availability */
.availability {
margin-top: 15px;
color: #00adb5; /* Matching teal for accent */
font-weight: bold;
}

/* Loading and error messages */
.text-error {
color: #ff5722; /* Bright red for error */
}

.loading-message {
color: #bbbbbb; /* Subtle gray for loading */
}

/* Buttons */
.action-button {
background-color: #00adb5;
color: white;
font-weight: bold;
text-transform: uppercase;
padding: 10px 20px;
border-radius: 6px;
transition: background-color 0.3s ease, box-shadow 0.3s ease;
}

.action-button:hover {
background-color: #007b7e;
box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.3);
}

.not-available {
font-size: 1.1rem;
color: #ff5722; /* Bright red for unavailable */
}

/* Divider */
.v-divider {
border-color: #444444;
}

/* Responsiveness */
.book-details {
padding: 15px;
}
</style>



