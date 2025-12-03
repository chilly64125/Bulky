<template>
  <div class="container py-4" style="max-width: 600px">
    <h2>{{ isEdit ? "編輯會員" : "新增會員" }}</h2>

    <form @submit.prevent="onSubmit" novalidate>
      <div class="mb-3">
        <label class="form-label">用戶名</label>
        <input
          v-model="values.username"
          type="text"
          autocomplete="username"
          class="form-control"
          :class="{ 'is-invalid': errors.username }"
          :disabled="isEdit"
        />
        <div v-if="errors.username" class="invalid-feedback">
          {{ errors.username }}
        </div>
      </div>

      <div class="mb-3">
        <label class="form-label">電郵</label>
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
        <label class="form-label">名字</label>
        <input v-model="values.firstName" type="text" class="form-control" />
      </div>

      <div class="mb-3">
        <label class="form-label">姓氏</label>
        <input v-model="values.lastName" type="text" class="form-control" />
      </div>

      <div class="mb-3">
        <label class="form-label">角色</label>
        <div class="form-check">
          <input
            v-model="values.roles"
            type="checkbox"
            value="Admin"
            class="form-check-input"
            id="adminRole"
          />
          <label class="form-check-label" for="adminRole">Admin</label>
        </div>
        <div class="form-check">
          <input
            v-model="values.roles"
            type="checkbox"
            value="Customer"
            class="form-check-input"
            id="customerRole"
          />
          <label class="form-check-label" for="customerRole">Customer</label>
        </div>
      </div>

      <div v-if="error" class="alert alert-danger">{{ error }}</div>

      <div class="d-flex gap-2">
        <button class="btn btn-primary" :disabled="loading">
          <span
            v-if="loading"
            class="spinner-border spinner-border-sm me-2"
          ></span>
          {{ isEdit ? "更新" : "新增" }}
        </button>
        <router-link to="/user" class="btn btn-secondary">返回</router-link>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import * as yup from "yup";
import type { User } from "@/types";
import { crudService } from "@/services/crudService";
import { useNotificationStore } from "@/stores/notificationStore";

const router = useRouter();
const route = useRoute();
const notificationStore = useNotificationStore();
const userService = crudService<User>("/user");

const loading = ref(false);
const error = ref<string | null>(null);
const isEdit = ref(!!route.params.id);

const values = reactive<Partial<User>>({
  username: "",
  email: "",
  firstName: "",
  lastName: "",
  roles: [],
});

const errors = reactive<any>({});

const schema = yup.object({
  username: yup.string().required("用戶名為必填"),
  email: yup.string().email("請輸入有效的電郵").required("電郵為必填"),
});

async function validate() {
  try {
    await schema.validate(values, { abortEarly: false });
    Object.keys(errors).forEach((k) => delete (errors as any)[k]);
    return true;
  } catch (err: any) {
    Object.keys(errors).forEach((k) => delete (errors as any)[k]);
    if (err.inner) {
      for (const e of err.inner) {
        if (e.path) (errors as any)[e.path] = e.message;
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
    if (isEdit.value && values.userId) {
      await userService.update(values.userId as any, values);
      notificationStore.success("會員已更新");
    } else {
      await userService.create(values);
      notificationStore.success("會員已新增");
    }
    router.push("/user");
  } catch (e: any) {
    error.value = e?.message || "操作失敗";
  } finally {
    loading.value = false;
  }
}

onMounted(async () => {
  if (isEdit.value && route.params.id) {
    try {
      const { data } = await userService.getById(route.params.id as any);
      Object.assign(values, data.data);
    } catch (e: any) {
      error.value = e?.message || "加載失敗";
    }
  }
});
</script>

<style scoped>
.container {
  margin-top: 1.5rem;
}
</style>
