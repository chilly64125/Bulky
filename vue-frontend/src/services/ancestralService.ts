import apiClient from './api'
import type { AncestralPosition, AncestralPositionQuery, ApiResponse, PaginatedResponse } from '@/types'

export const ancestralService = {
  getAll(page?: number, pageSize?: number) {
    return apiClient.get<ApiResponse<PaginatedResponse<AncestralPosition>>>('/ancestral', {
      params: { page, pageSize }
    })
  },

  getById(id: number) {
    return apiClient.get<ApiResponse<AncestralPosition>>(`/ancestral/${id}`)
  },

  create(data: AncestralPosition) {
    return apiClient.post<ApiResponse<AncestralPosition>>('/ancestral', data)
  },

  update(id: number, data: AncestralPosition) {
    return apiClient.put<ApiResponse<AncestralPosition>>(`/ancestral/${id}`, data)
  },

  delete(id: number) {
    return apiClient.delete(`/ancestral/${id}`)
  },

  query(queryParams: AncestralPositionQuery) {
    return apiClient.get<ApiResponse<PaginatedResponse<AncestralPosition>>>('/ancestral/positions/query', {
      params: queryParams
    })
  },

  getOccupancy() {
    return apiClient.get('/ancestral/occupancy')
  },

  import(file: File) {
    const formData = new FormData()
    formData.append('file', file)
    return apiClient.post('/ancestral/import', formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    })
  }
}
