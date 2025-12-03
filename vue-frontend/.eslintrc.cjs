module.exports = {
  root: true,
  env: {
    node: true,
    browser: true,
    es2024: true,
  },
  parser: "vue-eslint-parser",
  parserOptions: {
    parser: "@typescript-eslint/parser",
    ecmaVersion: 2024,
    sourceType: "module",
    extraFileExtensions: [".vue"],
  },
  plugins: ["vue", "@typescript-eslint"],
  extends: [
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended",
    "prettier",
  ],
  rules: {
    // project-specific rule overrides
    "vue/html-closing-bracket-newline": "off",
    "@typescript-eslint/no-explicit-any": "off",
  },
};
