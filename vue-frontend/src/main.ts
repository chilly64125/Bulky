import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import router from "./router";
import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.bundle";
import "bootstrap-icons/font/bootstrap-icons.css";
import "./assets/css/main.css";

// Check for server-injected data (from MVC Index1.cshtml)
const serverData = (window as any).__INITIAL_SERVER_DATA__;
if (serverData) {
  console.log("✅ Server-injected data detected:", serverData);
  // Store in sessionStorage for app-wide access
  sessionStorage.setItem("__INITIAL_SERVER_DATA__", JSON.stringify(serverData));
} else {
  console.log("ℹ️ No server-injected data found (normal for dev/SPA mode)");
}

const app = createApp(App);

app.use(createPinia());
app.use(router);

app.mount("#app");
