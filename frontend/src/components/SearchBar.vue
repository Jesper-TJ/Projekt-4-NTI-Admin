<script setup lang="ts">
import { ref } from "vue";

const props = defineProps({
  modelValue: String
})

const emit = defineEmits(['update:modelValue', 'search'])

const search = ref(props.modelValue || "");

function searched() {
  emit('search', search.value);
}
const test = ''
  console.log(test)
</script>

<template>
<v-text-field
  v-model="search"
  label="Search"
  prepend-inner-icon="mdi-magnify"
  variant="outlined"
  hide-details
  single-line
  @keyup="searched"
></v-text-field>
</template>
