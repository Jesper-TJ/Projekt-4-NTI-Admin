/**
 * plugins/vuetify.ts
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import "@mdi/font/css/materialdesignicons.css";
import "vuetify/styles";

// Composables
import { createVuetify, type ThemeDefinition } from "vuetify";

// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides

const ntiDarkTheme: ThemeDefinition =
{
  dark: true,
  colors: {
    background: "#43464b",
    surface: "#222831",
    primary: "#00adb5",
    secondary: "#008087",
    cta: "#4EAE52",
    error: "#C62B2B",
    info: "#1B76D0",
    success: "#4EAE52",
    warning: "#FA8D02",
  }
}

export default createVuetify({
  theme: {
    defaultTheme: 'ntiDarkTheme',
    themes: {
      ntiDarkTheme,
      light: {
        colors: {
          primary: "#1867C0",
          secondary: "#5CBBF6",
        },
      },
    },
  },
});
