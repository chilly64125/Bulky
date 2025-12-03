<template>
  <div class="container py-4" style="max-width: 520px">
    <h2 class="mb-3">註冊</h2>

    <form @submit.prevent="onSubmit" novalidate>
      <div class="mb-3">
        <label class="form-label">姓名</label>
        <input
          v-model="values.name"
          type="text"
          autocomplete="name"
          class="form-control"
          :class="{ 'is-invalid': errors.name }"
        />
        <div v-if="errors.name" class="invalid-feedback">{{ errors.name }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">電子郵件</label>
        <input
          v-model="values.email"
          type="email"
          autocomplete="email"
          class="form-control"
          :class="{ 'is-invalid': errors.email }"
        />
        <div v-if="errors.email" class="invalid-feedback">
          {{ errors.email }}
        </div>
      </div>

      <div class="mb-3">
        <label class="form-label">密碼</label>
        <input
          v-model="values.password"
          type="password"
          autocomplete="new-password"
          class="form-control"
          :class="{ 'is-invalid': errors.password }"
        />
        <div v-if="errors.password" class="invalid-feedback">
          {{ errors.password }}
        </div>
      </div>

      <div class="mb-3">
        <label class="form-label">確認密碼</label>
        <input
          v-model="values.confirmPassword"
          type="password"
          autocomplete="new-password"
          class="form-control"
          :class="{ 'is-invalid': errors.confirmPassword }"
        />
        <div v-if="errors.confirmPassword" class="invalid-feedback">
          {{ errors.confirmPassword }}
        </div>
      </div>

      <div class="d-flex justify-content-end mb-3">
        <button class="btn btn-success" :disabled="loading">
          <span
            v-if="loading"
            class="spinner-border spinner-border-sm me-2"
            role="status"
            aria-hidden="true"
          ></span>
          註冊
        </button>
      </div>

      <div v-if="error" class="alert alert-danger">{{ error }}</div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { reactive, ref } from "vue";
import { useRouter } from "vue-router";
import * as yup from "yup";
import { useAuthStore } from "../../stores/authStore";

const router = useRouter();
const authStore = useAuthStore();

const loading = ref(false);
const error = ref<string | null>(null);

const values = reactive({
  name: "",
  email: "",
  password: "",
  confirmPassword: "",
});
const errors = reactive<{
  name?: string;
  email?: string;
  password?: string;
  confirmPassword?: string;
}>({});

const schema = yup.object({
  name: yup.string().required("請輸入姓名"),
  email: yup.string().email("請輸入有效的電子郵件").required("電子郵件為必填"),
  password: yup.string().min(6, "密碼至少6位").required("密碼為必填"),
  confirmPassword: yup
    .string()
    .oneOf([yup.ref("password")], "密碼不一致")
    .required("請再次輸入密碼"),
});

async function validate() {
  try {
    await schema.validate(values, { abortEarly: false });
    Object.keys(errors).forEach((k) => delete (errors as any)[k]);
    return true;
  } catch (err: any) {
    Object.keys(errors).forEach((k) => delete (errors as any)[k]);
    if (err.inner && err.inner.length) {
      for (const e of err.inner) {
        if (e.path && e.message) (errors as any)[e.path] = e.message;
      }
    }
    return false;
  }
}

async function onSubmit() {
  error.value = null;
  const ok = await validate();
  if (!ok) return;

  loading.value = true;
  try {
    await authStore.register({
      name: values.name,
      email: values.email,
      password: values.password,
    });
    router.push("/login");
  } catch (e: any) {
    error.value = e?.message || "註冊失敗，請稍後再試";
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
