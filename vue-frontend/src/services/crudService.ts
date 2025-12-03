// Generic CRUD service factory
import apiClient from "./api";
import type { ApiResponse, PaginatedResponse } from "@/types";

// New named export `crudService` — used by other modules in the app.
export function crudService<T>(endpoint: string) {
  return {
    // common list alias used across views
    list(page?: number, pageSize?: number) {
      return apiClient.get<ApiResponse<PaginatedResponse<T>>>(endpoint, {
        params: { page, pageSize },
      });
    },

    // keep getAll for compatibility with older code
    getAll(page?: number, pageSize?: number) {
      return apiClient.get<ApiResponse<PaginatedResponse<T>>>(endpoint, {
        params: { page, pageSize },
      });
    },

    getById(id: number) {
      return apiClient.get<ApiResponse<T>>(`${endpoint}/${id}`);
    },

    create(data: T) {
      return apiClient.post<ApiResponse<T>>(endpoint, data);
    },

    update(id: number, data: T) {
      return apiClient.put<ApiResponse<T>>(`${endpoint}/${id}`, data);
    },

    delete(id: number) {
      return apiClient.delete(`${endpoint}/${id}`);
    },
  };
}

// Backwards-compatible alias for any code importing `createCrudService`
export function createCrudService<T>(endpoint: string) {
  return crudService<T>(endpoint);
}
