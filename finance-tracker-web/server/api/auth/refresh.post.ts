export default defineEventHandler(async (event) => {
  const refreshToken = getRefreshCookie(event)
  if (!refreshToken) {
    throw createError({ statusCode: 401, statusMessage: 'No refresh token present.' })
  }

  try {
    const response = await callBackendAuth(event, 'refresh', { refreshToken })
    setRefreshCookie(event, response.refreshToken)
    return toClientResponse(response)
  }
  catch (error) {
    clearRefreshCookie(event)
    throw error
  }
})
