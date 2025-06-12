<script setup lang="ts">
import { ref } from "vue";

const visible = ref(false);
</script>

<template>
  <v-card class="mx-auto pa-12 pb-8" elevation="0" max-width="448" rounded="lg">
    <v-img
      class="mx-auto my-6"
      max-width="228"
      src="../assets/ntiglogo.png"
    ></v-img>

    <div class="text-subtitle-1 text-medium-emphasis">Email address</div>

    <v-text-field
      density="compact"
      prepend-inner-icon="mdi-email-outline"
      variant="outlined"
    ></v-text-field>

    <div
      class="text-subtitle-1 text-medium-emphasis d-flex align-center justify-space-between"
    >
      Password

      <a
        class="text-caption text-decoration-none text-blue"
        href="#"
        rel="noopener noreferrer"
      >
        Forgot your password?</a
      >
    </div>

    <v-text-field
      :append-inner-icon="visible ? 'mdi-eye-off' : 'mdi-eye'"
      :type="visible ? 'text' : 'password'"
      density="compact"
      prepend-inner-icon="mdi-lock-outline"
      variant="outlined"
      @click:append-inner="visible = !visible"
    ></v-text-field>

    <v-btn block class="mb-8" size="x-large" rounded="xl" color="#111111">
      Log In
    </v-btn>
  </v-card>
</template>

<style scoped></style>
