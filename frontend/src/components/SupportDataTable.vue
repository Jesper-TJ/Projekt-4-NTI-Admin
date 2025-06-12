<template>
  <v-data-table :items="items" :headers="headers" show-expand hover>
    <template #top>
      <v-toolbar flat>
        <v-toolbar-title>Service Errands</v-toolbar-title>
        <v-btn v-if="user.admin">CREATE NEW SERVICE ERRAND</v-btn>
      </v-toolbar>
    </template>

    <template #expanded-row="{ columns, item }">
      <tr>
        <td :colspan="columns.length">
          <SupportDataTableCard
            :item="item"
            :user="user"

            :is-open="false"

          ></SupportDataTableCard>
        </td>
      </tr>
    </template>
  </v-data-table>
</template>

<script setup lang="ts">
import SupportDataTableCard from "./SupportDataTableCard.vue";
import { SupportErrand } from "@/views/Support.vue";

defineProps<{
  isOpen: boolean,
  items: SupportErrand[];
  user: {
    name: string;
    admin: boolean;
  };
}>();

//for datatable
const headers = [
  {
    key: "data-table-expand",
  },
  {
    title: "Name",
    key: "name",
  },
  {
    title: "Title",
    key: "title",
  },
  {
    title: "Status",
    key: "status",
  },
  {
    title: "Time Sent",
    key: "createdAt",
  },
];
</script>
