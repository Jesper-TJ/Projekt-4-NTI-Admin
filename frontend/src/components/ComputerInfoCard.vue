<template>
  <div class="horizontal-center">
    <v-row justify="center">
      <v-col cols="12" md="8" lg="6">
        <v-card class="owner-card">
          <div class="details-container">
            <h2 class="text-h5 owner-title">{{ ownerName }}</h2>
            <p class="text-subtitle-1 owner-info">
              {{ computerName }} - {{ ownerRole }} - {{ issue }}
            </p>
          </div>
          <v-divider class="my-4" />
          <v-card-actions class="actions">
            <v-btn class="action-button" @click="goToComputer(serial)">
              View Computer
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from "vue-router";

const router = useRouter();

defineProps<{
  ownerName: string;
  computerName: string;
  ownerRole: string;
  issue: string;
  serial: string;
}>();

const goToComputer = (serial: string) => {
  router.push({
    name: "Computer",
    params: { computer_id: serial },
  });
};
</script>

<style scoped>
/* Wrapper for horizontal centering */
.horizontal-center {
  display: flex;
  justify-content: center;
  padding: 10px; /* Add padding to avoid clipping */
  box-sizing: border-box; /* Include padding in size calculation */
  width: 117%; /* Ensure full width of the viewport */
}

/* General styles */
.owner-card {
  background-color: #222831; /* Dark gray */
  color: #eeeeee; /* Light gray text */
  border-radius: 12px;
  box-shadow: 0 6px 20px rgba(0, 0, 0, 0.7);
  padding: 20px;
  overflow: hidden;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.owner-card:hover {
  transform: scale(1.02);
  box-shadow: 0px 6px 15px rgba(0, 0, 0, 0.5);
}

/* Title and info */
.owner-title {
  font-size: 1.5rem;
  font-weight: bold;
  color: #00adb5; /* Teal accent */
}

.owner-info {
  margin-top: 10px;
  line-height: 1.6;
  color: #cfcfcf; /* Slightly muted for body text */
}

/* Divider */
.v-divider {
  border-color: #444444;
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

/* Responsiveness */
.details-container {
  padding: 15px;
}
</style>
