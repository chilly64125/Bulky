import { ref } from "vue";
import type { PaginatedResponse } from "@/types";

export function useDataFetch<T>(
  fetchFn: () => Promise<any>,
  options?: { immediate?: boolean }
) {
  const data = ref<T[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);
  const page = ref(1);
  const totalPages = ref(1);

  async function fetch() {
    loading.value = true;
    error.value = null;
    try {
      const { data: response } = await fetchFn();
      const result = response.data as PaginatedResponse<T>;
      data.value = result.items || [];
      totalPages.value = result.totalPages || 1;
    } catch (e: any) {
      error.value = e?.response?.data?.message || e?.message || "Fetch failed";
    } finally {
      loading.value = false;
    }
  }

  function nextPage() {
    if (page.value < totalPages.value) {
      page.value++;
      fetch();
    }
  }

  function prevPage() {
    if (page.value > 1) {
      page.value--;
      fetch();
    }
  }

  if (options?.immediate !== false) {
    fetch();
  }

  return {
    data,
    loading,
    error,
    page,
    totalPages,
    fetch,
    nextPage,
    prevPage,
  };
}
