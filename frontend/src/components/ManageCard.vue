<template>
  <v-card
    class="mx-auto pa-12 pb-8"
    elevation="8"
    max-width="450"
    drounded="lg"
  >
    <v-img
      class="fill-height"
      aspect-ratio="16/9"
      cover
      :src="imageSrc"
    ></v-img>

    <div>
      <div class="text-h6">
        {{ cardTitle }}
      </div>
      <ul>
        <li v-for="item in items" :key="item">{{ item }}</li>
      </ul>
    </div>

    <v-container>
      <v-row>
        <v-col>
          <v-btn color="blue">{{ manageButtonLabel }}</v-btn>
        </v-col>
        <v-col>
          <v-btn variant="outlined" color="red">
            {{ updatedStatusButtonLabel }}
          </v-btn>
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>

<script setup lang="ts">
  defineProps<{
    imageSrc: string,
    cardTitle: string,
    items: string[],
    manageButtonLabel: string,
    updatedStatusButtonLabel: string
  }>()
</script>
