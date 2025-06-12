module.exports = {
  root: true,
  env: {
    node: true,
  },
  extends: [
    "plugin:vue/vue3-recommended",
    "eslint:recommended",
    "@vue/eslint-config-typescript",
    "plugin:storybook/recommended",
    "plugin:vuetify/recommended",
    "prettier",
  ],
  rules: {
    "vue/multi-word-component-names": "off",
    // disables required default value for props
    "vue/require-default-prop": "off",
  },
};
