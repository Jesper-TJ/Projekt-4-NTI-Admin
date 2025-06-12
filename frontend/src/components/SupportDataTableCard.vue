<template>
  <v-card flat variant="outlined">
    <v-card-title>{{ item.title }}</v-card-title>
    <v-card-subtitle>{{ item.name }}</v-card-subtitle>
    <v-card-text class="text-subtitle-1">{{ item.description }}</v-card-text>
    <v-card-actions>
      <v-btn
      v-if="!isOpen"
        variant="tonal"
        @click="
          $router.push({
            name: 'SupportTicket',
            params: { ticket_id: item.id },
          })
        "
        >Open</v-btn
      >
      <v-btn
      v-if="isOpen"
      variant="tonal"
      @click="
      $router.push({
        name: 'Support',
      })
      "
      >Close</v-btn>
      <template v-if="user.admin">


          <v-btn v-if="item.status === 'Request'" variant="tonal"
            >mark as accepted</v-btn
          >
          <v-btn v-if="item.status === 'Accepted'" variant="tonal"
            >mark as complete</v-btn
          >
          <v-btn v-if="item.status === 'Complete'" variant="tonal"
            >delete errand</v-btn
          >

      </template>
    </v-card-actions>
  </v-card>
</template>

<script setup lang="ts">
import { SupportErrand } from "@/views/Support.vue";

defineProps<{
  item: SupportErrand;
  user: { name: string; admin: boolean };
  isOpen: boolean;
}>();
</script>
