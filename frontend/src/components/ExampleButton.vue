<!-- This component is here to show how storybook works, please don't remove. -->
<!-- Comments here will show up as descriptions in the storybook so don't skip them. -->

<template>
  <v-btn :color="color" :loading="loading">
    <!-- @slot To put any element inside the button-->
    <slot>
      {{ content }}
    </slot>
  </v-btn>
</template>

<script setup lang="ts">
import { withDefaults } from "vue";

export type Props = {
  /** Color of the button */
  color?: "primary" | "secondary" | "success" | "info" | "warning" | "error";

  /** To show a loading icon */
  loading?: boolean;

  /** the content of the button */
  content?: string;
};
withDefaults(defineProps<Props>(), {
  color: "primary",
  loading: false,
  content: "Button",
});
</script>
