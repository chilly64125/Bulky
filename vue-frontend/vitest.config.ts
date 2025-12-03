import { defineConfig } from "vitest/config";
import vue from "@vitejs/plugin-vue";
import { fileURLToPath } from "node:url";

export default defineConfig({
  plugins: [vue()],
  test: {
    environment: "jsdom",
    // setupFiles: ["./tests/setup.ts"],  // temporarily disabled to debug
    globals: true,
    include: ["tests/**/*.{test,spec}.{js,ts}"],
  },
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
});
