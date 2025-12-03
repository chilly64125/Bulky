import apiClient from './api'
import type { KindnessPosition, KindnessPositionQuery, ApiResponse, PaginatedResponse } from '@/types'

export const kindnessService = {
  getAll(page?: number, pageSize?: number) {
    return apiClient.get<ApiResponse<PaginatedResponse<KindnessPosition>>>('/kindness', {
      params: { page, pageSize }
    })
  },

  getById(id: number) {
    return apiClient.get<ApiResponse<KindnessPosition>>(`/kindness/${id}`)
  },

  create(data: KindnessPosition) {
    return apiClient.post<ApiResponse<KindnessPosition>>('/kindness', data)
  },

  update(id: number, data: KindnessPosition) {
    return apiClient.put<ApiResponse<KindnessPosition>>(`/kindness/${id}`, data)
  },

  delete(id: number) {
    return apiClient.delete(`/kindness/${id}`)
  },

  query(queryParams: KindnessPositionQuery) {
    return apiClient.get<ApiResponse<PaginatedResponse<KindnessPosition>>>('/kindness/positions/query', {
      params: queryParams
    })
  },

  getOccupancyByFloor(floor: number) {
    return apiClient.get(`/kindness/occupancy/${floor}`)
  },

  import(file: File) {
    const formData = new FormData()
    formData.append('file', file)
    return apiClient.post('/kindness/import', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })
  }
}
