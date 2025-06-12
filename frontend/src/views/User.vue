

<script lang="ts" setup>
import ManageCard from "@/components/ManageCard.vue";
import router from "@/router";
import {useUserStore} from "@/store/UserStore"
import { stringify } from "querystring";

const props = defineProps( {
  user_id: {
    type: String,
    required: true
  }
})


const UserStore = useUserStore()

const user = UserStore.userById(parseInt(props.user_id))

alert(JSON.stringify(user))


const firstImageSrc =
  "https://content.instructables.com/F4N/K84M/JDUKKGUH/F4NK84MJDUKKGUH.jpg?auto=webp&frame=1&width=1024&height=1024&fit=bounds&md=9b3dc16f37f12c8ec5aaaca97c447292";
const firstCardTitle = "Computer";
const firstItems = ["Computer Status", "Service Overview", "Service Request"];
const firstManageButtonLabel = "Manage";
const firstUpdatedStatusButtonLabel = "Updated Status";

const secoundImageSrc =
  "https://tutoringsolutionsgroup.com/wp-content/uploads/2022/06/shutterstock_1937995444-750x500.jpg";
const secoundCardTitle = "Library";
const secoundItems = ["Library books overview", "Course Books"];
const secoundManageButtonLabel = "View";
const secoundUpdatedStatusButtonLabel = "Overdue Books";
</script>

<template>
  <div class="d-flex justify-center align-center fill-height">
    <h1>
       {{ user?.name }}
    </h1>
    <ManageCard
      :image-src="firstImageSrc"
      :card-title="firstCardTitle"
      :items="firstItems"
      :manage-button-label="firstManageButtonLabel"
      :updated-status-button-label="firstUpdatedStatusButtonLabel"
    />
    <ManageCard
      :image-src="secoundImageSrc"
      :card-title="secoundCardTitle"
      :items="secoundItems"
      :manage-button-label="secoundManageButtonLabel"
      :updated-status-button-label="secoundUpdatedStatusButtonLabel"
    />
  </div>
</template>