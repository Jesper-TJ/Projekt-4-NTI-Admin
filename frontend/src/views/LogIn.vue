<script setup lang="ts">
import LogInForm from "@/components/LogInForm.vue";
import GoogleLoginButton from "../components/GoogleLoginButton.vue";
import { ref, onMounted } from "vue";

// Define props, reactive state, and methods directly in the setup block
const googleClientId = ref<string | null>(null); // Replace with your Google OAuth Client ID

onMounted(async () => {
  try {
    const response = await fetch("../../config.json"); // Fetch the file
    const config = await response.json(); // Parse the JSON
    googleClientId.value = config.googleClientId; // Access the Client ID
  } catch (error) {
    console.error("Error loading configuration:", error);
  }
});

// Handle login success
function handleGoogleLogin(credential: any) {
  console.log("Google credential received:", credential);
  // Process the credential, e.g., send it to your backend
}

// Handle login error
function handleLoginError() {
  console.error("Google login failed");
}
</script>

<template>
  <!-- NTI logga -->
  <v-img
    class="mx-auto my-6"
    max-width="228"
    src="../assets/ntiglogo.png"
  ></v-img>

  <google-login-button
    v-if="googleClientId"
    :clientId="googleClientId"
    :onLoginSuccess="handleGoogleLogin"
    :onLoginError="handleLoginError"
  />
</template>

<style scoped>
 
</style>
