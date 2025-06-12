<script setup lang="ts">
import router from "@/router";
import { ref } from "vue";
import {useUserStore} from "@/store/UserStore"
const UserStore = useUserStore()
UserStore.fetchUsers()
const users = ref(UserStore.allUsers)
</script>


<template>    
<h1>Users</h1>
  <div v-for="user in users" :key="user.id">

  <v-btn
    @click="
      router.push({
        name: 'User',
        params: {
          user_id: user.id,
        },
      })
    "
    >{{ user.name }}</v-btn
  >
  </div>


</template>


