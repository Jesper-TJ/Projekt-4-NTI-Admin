// Utilities
import { defineStore } from "pinia";

// Port for production server (443 https default port)
// When hosting the backend in an actual server (during production/staging)
const port = import.meta.env.PROD ? "443" : "3002";

// Change to IP of server when running in production/staging
const baseUrl = "localhost:" + port;

export const useAppStore = defineStore("app", {
  state: () => ({
    baseUrl: baseUrl,
  }),
});
