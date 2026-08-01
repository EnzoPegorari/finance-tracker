export default defineEventHandler(async (event) => {
  const refreshToken = getRefreshCookie(event)
  const config = useRuntimeConfig(event)

  if (refreshToken) {
    try {
      await $fetch(`${config.apiBase}/auth/logout`, {
        method: 'POST',
        body: { refreshToken },
      })
    }
    catch {
      // Refresh token may already be invalid/expired — clearing the cookie is enough either way.
    }
  }

  clearRefreshCookie(event)
  return { success: true }
})
