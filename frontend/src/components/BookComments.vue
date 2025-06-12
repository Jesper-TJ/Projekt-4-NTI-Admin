<template>
  <v-row justify="center" :key="componentKey">
    <v-col cols="12" md="8" lg="6">
      <v-card class="book-card">
        
        <v-switch
          v-model="anonymous"
          class="toggle-switch ma-5 d-flex align-center"
          inset
          color="#00adb5"
          :true-value="true"
          :false-value="false"
          label="Stay anonymous to the public"
        ></v-switch>

        <!-- Review Submission Section -->
        <v-row class="ma-5 d-flex align-center">
          <v-text-field
            v-model="newReview"
            label="Post review"
            class="ma-auto mr-5 add-comment"
            hide-details
          ></v-text-field>
          
          <v-btn v-if="selectedSliderValue > 0 " prepend-icon="mdi-plus-circle" class="action-button" @click="submitReview">Submit</v-btn>
          <v-btn v-else="" class="action-button-inactive">Select Rating</v-btn>
        </v-row>
        <v-row>                              
          <div class="stars" @mouseenter.prevent="startDrag" @mousemove="onDrag" @mouseup="selectSliderValue" @mouseleave="onExit">
            <v-icon 
              v-for="n in 5" 
              :id="n"
              :key="n" 
              :size="38" 
              color="yellow" 
              :icon="getStarIcon(n)"
              class="clickable"
            ></v-icon>
          </div>

        </v-row>

        <!-- Reviews Display Section -->
        <v-card-text>
          <v-divider class="my-4" />
          <h3 class="reviews-title">Reviews ({{ bookReviews.reviews.length }})</h3>
          <v-table height="300px" class="comments" v-if="bookReviews.reviews?.length">
            <tbody>
              <tr v-for="comment in bookReviews.reviews" :key="comment.id">
                <td>
                  <v-col class="comment-box">
                    <p v-if="!comment.anonymous"><strong>User:</strong> {{ comment.userName }}</p>
                    <p v-else><strong>User:</strong> Anonymous</p>
                    <p><strong>Rating:</strong> {{ comment.rating }}</p>
                    <p><strong>Content:</strong> {{ comment.content }}</p>
                  </v-col>
                </td>
              </tr>
            </tbody>
          </v-table>
          <p v-else class="no-reviews">No reviews available.</p>
        </v-card-text>
      </v-card>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { IBook, useBookStore } from '@/store/BookStore';
import Books from '@/views/Books.vue';
import router from '@/router';

const props = defineProps<{
  bookReviews: IBook;
  bookid: number;
}>();

const emit = defineEmits(['refresh']);

const componentKey = ref(0)

// Local state for new review input
const newReview = ref('');
const anonymous = ref(false);

// Store instance to manage global state (if needed)
const bookStore = useBookStore();

const userId = ref(1);
const userName = ref("Philip Branting")

const sliderValue = ref(0)
const selectedSliderValue = ref(0)
const isDragging = ref(false)

function refreshComponent(){
  componentKey.value += 1;
}


function getStarIcon(starIndex: number) {
  const value = sliderValue.value;
  if (value >= starIndex) {
    return 'mdi-star'; // Full star
  } else if (value >= starIndex - 0.5) {
    return 'mdi-star-half-full'; // Half star
  } else {
    return 'mdi-star-outline'; // Empty star
  }
}




function updateStars(starIndex: number) {
      sliderValue.value = starIndex;
    }
function startDrag(event: Event) {
      isDragging.value = true;
      updateFromEvent(event);
    }
function onDrag(event: Event) {
      if (isDragging.value) {
        updateFromEvent(event);
      }
    }

function selectSliderValue() {
  selectedSliderValue.value = sliderValue.value;
  endDrag();
}

function endDrag() {
      isDragging.value = false;
    }

    function updateFromEvent(event: Event) {
      const container = event.target as HTMLElement; // Cast to HTMLElement
      if (!container) return; // Guard clause
      const rect = container.getBoundingClientRect();
      const x = event instanceof MouseEvent ? event.clientX : 0; // Ensure it's a MouseEvent
      const offsetX = x - rect.left;

      sliderValue.value = Number(container.id) - 1 + (offsetX < 16.7 ? 0.5 : 1) ; // Round up to the nearest whole star
    }

    function onExit(){
      endDrag();
      sliderValue.value = selectedSliderValue.value;
    }



/**
 * Handles review submission
 */
async function submitReview() {
  if (!newReview.value.trim()) {
    alert('Please enter a review before submitting.');
    return;
  }

  try {
    const newComment = {
      id: 12312834728347,
      username: userName.value,
      rating: selectedSliderValue.value,
      createdAt: "",
      bookTitleId: props.bookid,
      anonymous: anonymous.value ? true : false,
      content: newReview.value,
    };

    console.log(newComment)

    
    const success = !await bookStore.postReview(newComment, Number(props.bookid));
    if (success) {
      newReview.value = '';
      sliderValue.value = 0;
      selectedSliderValue.value = 0;

      emit('refresh');
      refreshComponent();
    } else {
      alert('Failed to submit review. Please try again.');
    }
  } catch (error) {
    console.error('Error submitting review:', error);
    alert('Failed to submit review. Please try again.');
  }
}
</script>

<style scoped>
.stars {
  display: flex;
  justify-content: center;
  margin-left: 25px;
}

.clickable {
  cursor: pointer;
}

/********* Toggle Switch Styling *********/
.toggle-switch {
  margin-left: 17px;
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

/* Card Styles */
.book-card {
  background-color: #222831;
  color: #eeeeee;
  border-radius: 12px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.7);
  padding: 20px;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

/* Review Form */
.add-comment {
  flex: 1;
}
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
.action-button-inactive:hover {
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.3);
  background-color: rgba(40, 40, 40, 0.6);
}

.action-button-inactive {
  box-shadow: 0px 4px 10px rgba(0, 0, 0, 0.3);
  background-color: rgba(70, 70, 70, 0.6);
  color: #00adb5;
}


/* Reviews Section */
.reviews-title {
  font-size: 1.5rem;
  font-weight: bold;
  margin-bottom: 10px;
  color: #00adb5;
}
.comments {
  padding-top: 10px;
  background-color: #393e46;
  border-radius: 8px;
  overflow-y: auto;
}
p {
  color: white;
}
.comment-box {
  
  margin-top: 10px;
  padding: 10px;
  background-color: #222831;
  margin-bottom: 10px;
  border-radius: 6px;
  box-shadow: 0px 2px 5px rgba(0, 0, 0, 0.5);
}
.no-reviews {
  color: #bbbbbb;
  font-style: italic;
  text-align: center;
}
</style>
