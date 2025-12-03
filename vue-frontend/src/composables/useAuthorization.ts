import { computed } from 'vue'
import { useAuthStore } from '@/stores/authStore'

export function useAuthorization() {
  const authStore = useAuthStore()

  const isAdmin = computed(() => authStore.isAdmin)
  const isCustomer = computed(() => authStore.isCustomer)
  const isAuthenticated = computed(() => authStore.isAuthenticated)

  function hasRole(role: string): boolean {
    return authStore.hasRole(role)
  }

  function requireRole(role: string): boolean {
    if (!authStore.isAuthenticated) return false
    return authStore.hasRole(role)
  }

  function canAccessModule(module: string): boolean {
    // Admin can access everything
    if (isAdmin.value) return true

    // Customer can access: home, kindness query, ancestral query
    if (isCustomer.value) {
      const allowedModules = ['home', 'ancestral/query', 'kindness/query']
      return allowedModules.includes(module)
    }

    // Guest cannot access any protected modules
    return false
  }

  return {
    isAdmin,
    isCustomer,
    isAuthenticated,
    hasRole,
    requireRole,
    canAccessModule
  }
}
