<script setup lang="ts">
import router from "@/router";
import { ref } from "vue";

const tabs = ['Users', 'Library', 'Computers', 'Support', 'Access Cards'];
const currentTab = ref<string>((router.currentRoute.value.name as string) || tabs[0]);

const navigate = (tab: string) => {
  currentTab.value = tab;
  router.push({ name: tab });
};

export type Props = {
  /** If the user is signed in or not */
  signedIn?: boolean;
};

withDefaults(defineProps<Props>(), {
  signedIn: false,
});

const emit = defineEmits(['signin-pressed']);

const notifications = [
  {
    title: "Service på din dator klar",
  },
  {
    title: "Titel ska lämnas in imorgon",
  },
];
</script>

<template>
  <v-app-bar class="header-bar">
    <!-- Logo on the left -->
    <v-list-item @click="$router.push({ name: 'Home' })">
      <img
        src="https://ntigymnasiet.se/wp-content/uploads/2023/02/logotype-vit.svg"
        class="logotype-image"
      />
    </v-list-item>

    <v-divider vertical />

    <!-- Centered Tabs -->
    <v-spacer />
    <div class="tabs-container">
      <v-tabs v-model="currentTab" class="header-tabs" background-color="#222831">
        <v-tab
          v-for="tab in tabs"
          :key="tab"
          :value="tab"
          :class="{'active-tab': currentTab === tab}"
          @click="navigate(tab)"
        >
          {{ tab }}
        </v-tab>
      </v-tabs>
    </div>
    <v-spacer />

    <!-- Manage, Notifications, Login on the right -->
    <v-btn>
      <v-list-item title="Manage" />
    </v-btn>

    <v-menu transition="slide-y-transition">
      <template #activator="{ props }">
        <v-btn v-bind="props">
          <v-list-item title="Notifications" />
          <span class="material-symbols-outlined">notifications</span>
        </v-btn>
      </template>
      <v-list>
        <v-list-item
          v-for="notification in notifications"
          :key="notification.title"
        >
          <v-list-item-title>{{ notification.title }}</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-menu>

    <v-btn
      :append-icon="signedIn ? 'mdi-logout' : 'mdi-login'"
      @click="() => {
        $emit('signin-pressed');
        $router.push({ name: 'Login' });
      }"
    >
      {{ signedIn ? "Logout" : "Login" }}
    </v-btn>
  </v-app-bar>
</template>


<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:opsz,wght,FILL,GRAD@24,400,0,0");

.header-bar {
  background-color: #222831 !important;
  color: #00adb5;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.logotype-image {
  display: flex;
  height: 40px;
}

/* Tabs container */
.tabs-container {
  flex-grow: 1;
  display: flex;
  justify-content: center;
}

/* Header Tabs */
.header-tabs {
  background-color: #222831;
}

.active-tab {
  background-color: #222831;
  color: white;
  font-weight: bold;
  border-bottom: 3px solid #00adb5;
}

/* Override default button styling */
.v-btn {
  color: #00adb5;
}

.v-btn:hover {
  background-color: #e0f7fa;
}
</style>
