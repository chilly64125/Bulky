import apiClient from './api'
import type { LoginRequest, RegisterRequest, AuthResponse, User } from '@/types'

export const authService = {
  login(credentials: LoginRequest) {
    return apiClient.post<AuthResponse>('/auth/login', credentials)
  },

  register(data: RegisterRequest) {
    return apiClient.post<AuthResponse>('/auth/register', data)
  },

  logout() {
    return apiClient.post('/auth/logout')
  },

  getCurrentUser() {
    return apiClient.get<User>('/auth/current-user')
  },

  refreshToken(refreshToken: string) {
    return apiClient.post('/auth/refresh-token', { refreshToken })
  },

  changePassword(oldPassword: string, newPassword: string) {
    return apiClient.post('/auth/change-password', {
      oldPassword,
      newPassword
    })
  }
}
