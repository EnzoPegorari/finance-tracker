import { useAuthStore } from '~/stores/auth'

export default defineNuxtRouteMiddleware(async () => {
  const authStore = useAuthStore()

  if (authStore.isAuthenticated) {
    return
  }

  const restored = await authStore.refresh()
  if (!restored) {
    return navigateTo('/login')
  }
})
