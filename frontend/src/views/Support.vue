<template>
  <support-data-table :is-open="false" :items="errands" :user="user"></support-data-table>
</template>

<script setup lang="ts">
import SupportDataTable from "@/components/SupportDataTable.vue";
import { useSupportErrandStore } from "@/store/SupportErrandStore";

export interface SupportErrand {
  id: string;
  name: string;
  title: string;
  description: string;
  createdAt: string;
  status: string;
}

const user = {
  name: "Nicolai Zackrisson",
  admin: false,
};

const SupportErrandStore = useSupportErrandStore();
SupportErrandStore.fetchSupportErrands();
SupportErrandStore.filter(user);

const errands: SupportErrand[] = SupportErrandStore.all;
</script>
