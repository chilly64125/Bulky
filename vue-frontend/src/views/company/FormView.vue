<template>
  <div class="container py-4" style="max-width: 600px">
    <h2>{{ isEdit ? "編輯宗親會" : "新增宗親會" }}</h2>

    <form @submit.prevent="onSubmit" novalidate>
      <div class="mb-3">
        <label class="form-label">名稱</label>
        <input
          v-model="values.name"
          type="text"
          class="form-control"
          :class="{ 'is-invalid': errors.name }"
        />
        <div v-if="errors.name" class="invalid-feedback">{{ errors.name }}</div>
      </div>

      <div class="mb-3">
        <label class="form-label">地址</label>
        <input v-model="values.address" type="text" class="form-control" />
      </div>

      <div class="mb-3">
        <label class="form-label">城市</label>
        <input v-model="values.city" type="text" class="form-control" />
      </div>

      <div class="mb-3">
        <label class="form-label">電話</label>
        <input v-model="values.phone" type="tel" class="form-control" />
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

      <div v-if="error" class="alert alert-danger">{{ error }}</div>

      <div class="d-flex gap-2">
        <button class="btn btn-primary" :disabled="loading">
          <span
            v-if="loading"
            class="spinner-border spinner-border-sm me-2"
          ></span>
          {{ isEdit ? "更新" : "新增" }}
        </button>
        <router-link to="/app/company" class="btn btn-secondary">返回</router-link>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from "vue";
import { useRouter, useRoute } from "vue-router";
import * as yup from "yup";
import type { Company } from "@/types";
import { crudService } from "@/services/crudService";
import { useNotificationStore } from "@/stores/notificationStore";

const router = useRouter();
const route = useRoute();
const notificationStore = useNotificationStore();
const companyService = crudService<Company>("/company");

const loading = ref(false);
const error = ref<string | null>(null);
const isEdit = ref(!!route.params.id);

const values = reactive<Partial<Company>>({
  name: "",
  address: "",
  city: "",
  phone: "",
  email: "",
});

const errors = reactive<any>({});

const schema = yup.object({
  name: yup.string().required("名稱為必填"),
  email: yup.string().email("請輸入有效的電郵"),
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
    if (isEdit.value && values.companyId) {
      await companyService.update(values.companyId, values);
      notificationStore.success("宗親會已更新");
    } else {
      await companyService.create(values);
      notificationStore.success("宗親會已新增");
    }
    router.push("/company");
  } catch (e: any) {
    error.value = e?.message || "操作失敗";
  } finally {
    loading.value = false;
  }
}

onMounted(async () => {
  if (isEdit.value && route.params.id) {
    try {
      const { data } = await companyService.getById(Number(route.params.id));
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
