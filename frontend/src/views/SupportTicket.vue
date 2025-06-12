<script setup lang="ts">
import SupportDataTableCard from '@/components/SupportDataTableCard.vue';

import { SupportErrand } from './Support.vue';
const user =
{
  name: "Nicolai Zackrisson",
  admin: true,
};
const item: SupportErrand =
{
  id: "1",
  name: "Nicolai Zackrisson",
  title: "Broken Ipad",
  description: "I smashed my ipad with a hammer!",
  createdAt: "11:00:28",
  status: "Request",
};
</script>

<template>

  <SupportDataTableCard
  :item="item"
  :user="user"
  :is-open="true"
  />

</template>

<style scoped></style>
