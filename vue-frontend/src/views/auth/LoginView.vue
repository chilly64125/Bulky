<template>
  <div class="container py-4" style="max-width: 420px">
    <h2 class="mb-3">登入</h2>

    <form @submit.prevent="onSubmit" novalidate>
      <div class="mb-3">
        <label class="form-label">電子郵件</label>
        <input v-model="values.email" type="email" autocomplete="username" class="form-control"
          :class="{ 'is-invalid': errors.email }" />
        <div v-if="errors.email" class="invalid-feedback">
          {{ errors.email }}
        </div>
      </div>

      <div class="mb-3">
        <label class="form-label">密碼</label>
        <input v-model="values.password" type="password" autocomplete="current-password" class="form-control"
          :class="{ 'is-invalid': errors.password }" />
        <div v-if="errors.password" class="invalid-feedback">
          {{ errors.password }}
        </div>
      </div>

      <div class="d-flex justify-content-between align-items-center mb-3">
        <router-link to="/register">註冊帳號</router-link>
        <button class="btn btn-primary" :disabled="loading">
          <span v-if="loading" class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
          登入
        </button>
      </div>

      <div v-if="error" class="alert alert-danger alert-dismissible fade show d-flex align-items-start" role="alert">
        <i class="bi bi-exclamation-circle me-2 flex-shrink-0 mt-1"></i>
        <div class="flex-grow-1">
          <strong>登入失敗</strong>
          <p class="mb-0 mt-1">{{ error }}</p>
          <small class="text-muted d-block mt-2" v-html="errorHint"></small>
        </div>
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref, computed } from "vue";
import { useRouter } from "vue-router";
import * as yup from "yup";
import { useAuthStore } from "../../stores/authStore";

const router = useRouter();
const authStore = useAuthStore();

const loading = ref(false);
const error = ref<string | null>(null);
const errorCode = ref<string | null>(null);

const values = reactive({ email: "", password: "" });
const errors = reactive<{ email?: string; password?: string }>({});

const schema = yup.object({
  email: yup.string().email("請輸入有效的電子郵件").required("電子郵件為必填"),
  password: yup.string().min(6, "密碼至少6位").required("密碼為必填"),
});

// Compute error hint based on error code
const errorHint = computed(() => {
  switch (errorCode.value) {
    case "INVALID_CREDENTIALS":
      return "請檢查您的電子郵件和密碼是否正確。";
    case "USER_NOT_FOUND":
      return "此電子郵件尚未註冊。請 <a href='/register'>建立新帳號</a>。";
    case "INVALID_PASSWORD":
      return "密碼錯誤。請稍後重試，或 <a href='#'>重設密碼</a>。";
    case "ACCOUNT_LOCKED":
      return "帳號已被鎖定，請稍後再試或聯絡客服。";
    case "ACCOUNT_DISABLED":
      return "此帳號已被停用。請聯絡客服以取得協助。";
    case "EMAIL_NOT_CONFIRMED":
      return "您需要先驗證電子郵件。請檢查您的收件箱中的驗證信件。";
    case "NETWORK_ERROR":
      return "網路連線失敗。請檢查您的網路連線並重試。";
    case "SERVER_ERROR":
      return "伺服器出現問題。請稍後再試。";
    default:
      return "如果問題持續發生，請聯絡客服尋求協助。";
  }
});

async function validate() {
  try {
    await schema.validate(values, { abortEarly: false });
    errors.email = undefined;
    errors.password = undefined;
    return true;
  } catch (err: any) {
    errors.email = undefined;
    errors.password = undefined;
    if (err.inner && err.inner.length) {
      for (const e of err.inner) {
        if (e.path && e.message) (errors as any)[e.path] = e.message;
      }
    }
    return false;
  }
}

function mapErrorToCode(errorMessage: string): string {
  if (!errorMessage) return "UNKNOWN";

  const lower = errorMessage.toLowerCase();

  if (lower.includes("invalid") && lower.includes("credential")) return "INVALID_CREDENTIALS";
  if (lower.includes("user") && lower.includes("not") && lower.includes("found")) return "USER_NOT_FOUND";
  if (lower.includes("password") && lower.includes("incorrect")) return "INVALID_PASSWORD";
  if (lower.includes("locked")) return "ACCOUNT_LOCKED";
  if (lower.includes("disabled")) return "ACCOUNT_DISABLED";
  if (lower.includes("email") && lower.includes("confirm")) return "EMAIL_NOT_CONFIRMED";
  if (lower.includes("network") || lower.includes("timeout")) return "NETWORK_ERROR";
  if (lower.includes("server")) return "SERVER_ERROR";

  // Default to invalid credentials for generic failures
  return "INVALID_CREDENTIALS";
}

async function onSubmit() {
  error.value = null;
  errorCode.value = null;
  const ok = await validate();
  if (!ok) return;

  loading.value = true;
  try {
    await authStore.login({ username: values.email, password: values.password });

    // Redirect based on role after successful login
    if (authStore.isAdmin) {
      router.push("/app/admin");
    } else if (authStore.isCustomer) {
      router.push("/app/customer");
    } else {
      router.push("/app/customer"); // Default to customer for safety
    }
  } catch (e: any) {
    const message = e?.message || "登入失敗，請稍後再試";
    error.value = message;
    errorCode.value = mapErrorToCode(message);
  } finally {
    loading.value = false;
  }
}
</script>

<style scoped>
.container {
  margin-top: 3rem;
}
</style>
