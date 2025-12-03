import apiClient from './api'
import type { AppConfig } from '@/types'

const CONFIG_CACHE_KEY = 'app_config'
const CONFIG_CACHE_TTL = 60 * 60 * 1000 // 1 hour

export const configService = {
  async getConfig(): Promise<AppConfig> {
    // Check cache first
    const cached = localStorage.getItem(CONFIG_CACHE_KEY)
    if (cached) {
      const { data, timestamp } = JSON.parse(cached)
      if (Date.now() - timestamp < CONFIG_CACHE_TTL) {
        return data
      }
    }

    // Fetch from API
    const response = await apiClient.get<AppConfig>('/config/app')
    const config = response.data

    // Cache result
    localStorage.setItem(
      CONFIG_CACHE_KEY,
      JSON.stringify({
        data: config,
        timestamp: Date.now()
      })
    )

    return config
  },

  clearConfigCache() {
    localStorage.removeItem(CONFIG_CACHE_KEY)
  }
}
