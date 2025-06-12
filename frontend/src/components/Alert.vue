<template>
    <v-snackbar
    :model-value="displayState"
    :timeout="alertTime"
    timer="white"
    color="warning"
    @update:model-value="updateDisplayState"
    >
    
    {{ text }}

    <template v-slot:actions>
        <v-btn
        color="white"
        variant="text"
        @click="closeAlert"
        >
        Close
        </v-btn>
    </template>
    </v-snackbar>
</template>
<script setup lang="ts">

defineProps<{
    alertTime: number,
    text: string,
    displayState: boolean
}>();

const emit = defineEmits(['update:displayState']);  // Define event emitter

// Emit event to update parent state
function updateDisplayState(value: boolean) {
  emit('update:displayState', value);
}

// Close button handler
function closeAlert() {
  emit('update:displayState', false);
}
</script>