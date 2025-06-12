<template>
  <NavBar :signed-in="signedIn" @signin-pressed="signIn()"/>
  <v-main class="mt-16">
    <router-view />
  </v-main>
</template>

<script lang="ts" setup>
import { ref } from "vue";
let signedIn = ref(false)
function signIn() {
  signedIn.value = !signedIn.value
}
import NavBar from "@/components/NavBar.vue";
import { SignedIn } from "@/stories/NavBar.stories";
</script>
