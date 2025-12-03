import { defineStore } from "pinia";
import { ref } from "vue";
import type { Toast } from "@/types";
import { v4 as uuidv4 } from "uuid";

export const useNotificationStore = defineStore("notification", () => {
  const toasts = ref<Toast[]>([]);

  const addToast = (
    message: string,
    type: "success" | "error" | "info" | "warning" = "info",
    duration = 3000
  ) => {
    const id = uuidv4();
    const toast: Toast = {
      id,
      message,
      type,
      duration,
    };
    toasts.value.push(toast);

    if (duration > 0) {
      setTimeout(() => removeToast(id), duration);
    }

    return id;
  };

  const removeToast = (id: string) => {
    const index = toasts.value.findIndex((t) => t.id === id);
    if (index > -1) {
      toasts.value.splice(index, 1);
    }
  };

  const clearAll = () => {
    toasts.value = [];
  };

  const success = (message: string, duration?: number) =>
    addToast(message, "success", duration);
  const error = (message: string, duration?: number) =>
    addToast(message, "error", duration);
  const info = (message: string, duration?: number) =>
    addToast(message, "info", duration);
  const warning = (message: string, duration?: number) =>
    addToast(message, "warning", duration);

  return {
    toasts,
    addToast,
    removeToast,
    clearAll,
    success,
    error,
    info,
    warning,
  };
});
