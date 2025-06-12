<template>
  <v-data-table :items="computers" :headers="headers">
    <template v-slot:item="{ item }">
      <ComputersInfoCard
        :ownerName="item.computerlog.user_name"
        :computerName="item.computer.name"
        :ownerRole="getOwnerRole(item)"
        :issue="item.computer.damage"
        :serial="item.computer.serial"
      />
    </template>
  </v-data-table>
</template>

<script lang="ts" setup>
import { computed } from "vue";
import { useComputerStore } from "@/store/ComputerStore";
import ComputersInfoCard from "@/components/ComputerInfoCard.vue";

const computerStore = useComputerStore();
computerStore.fetchComputers();

const computers = computed(() => computerStore.allComputers);

const headers = [
  { text: "Computer Name", value: "computer.name" },
  { text: "Owner", value: "computerlog.user_name" },
  { text: "Damage Status", value: "computer.damage" },
];

const getOwnerRole = (item: { computer: { status: string } }) => {
  return item.computer.status === "In Use" ? "Active User" : "Inactive";
};
</script>
