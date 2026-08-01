import type { FetchOptions } from 'ofetch'
import { ofetch } from 'ofetch'
import { useAuthStore } from '~/stores/auth'

export async function apiFetch<T>(path: string, options: FetchOptions<'json'> = {}): Promise<T> {
  const config = useRuntimeConfig()
  const authStore = useAuthStore()

  const run = () =>
    ofetch<T>(path, {
      baseURL: config.public.apiBase,
      ...options,
      headers: {
        ...options.headers,
        ...(authStore.accessToken ? { Authorization: `Bearer ${authStore.accessToken}` } : {}),
      },
    })

  try {
    return await run()
  }
  catch (error: any) {
    if (error?.response?.status === 401 && authStore.accessToken) {
      const refreshed = await authStore.refresh()
      if (refreshed) {
        return await run()
      }
      await navigateTo('/login')
    }
    throw error
  }
}
