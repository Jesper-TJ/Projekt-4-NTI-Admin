<template>
    <v-expansion-panel>
        <v-expansion-panel-title>
            <template v-slot:default="{ expanded }">
                <v-row no-gutters>
                <v-col class="d-flex justify-start" cols="4">
                    {{ title }}
                </v-col>
                <v-col
                    cols="8"
                >
                    <v-fade-transition leave-absolute>
                    <span
                        v-if="expanded"
                        key="0"
                    >
                        {{ subTitle }}
                    </span>
                    </v-fade-transition>
                </v-col>
                </v-row>
            </template>
        </v-expansion-panel-title>
        <v-expansion-panel-text>
            <slot></slot>
        </v-expansion-panel-text>
    </v-expansion-panel>
</template>
<script setup lang="ts">

defineProps<{
    title: string,
    subTitle?: string,
}>();

</script>