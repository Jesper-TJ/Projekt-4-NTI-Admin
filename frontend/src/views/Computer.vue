<script setup lang="ts">
import { useRoute } from "vue-router";
import { useComputerStore } from "@/store/ComputerStore";
import ComputerInfoCard from '@/components/DetailedComputerInfoCard.vue';

const computerStore = useComputerStore();

const route = useRoute();
const computer_id = route.params.computer_id;

computerStore.fetchComputers()

const data = computerStore.getComputer(String(computer_id));

</script>
<template>
  <ComputerInfoCard :data="data" />
</template>