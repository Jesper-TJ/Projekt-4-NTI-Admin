import { setup, Preview } from "@storybook/vue3";
import { registerPlugins } from "../src/plugins";
import { VApp } from "vuetify/components";

setup((app) => {
  // Registers your app's plugins into Storybook
  registerPlugins(app);
});

// // Puts a v-app around all stories so that all vuetify components work
// // Currently removed because we couldn't fix a sizing issue
// const preview: Preview = {
//   decorators: [
//     (story) => ({
//       components: { story, VApp },
//       template: `
//             <v-app style="display: block;">
//                 <story />
//             </v-app>
//             `,
//     }),
//   ],
// };

// export default preview;
