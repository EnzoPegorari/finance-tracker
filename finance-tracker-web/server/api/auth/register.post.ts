export default defineEventHandler(async (event) => {
  const body = await readBody(event)

  const response = await callBackendAuth(event, 'register', body)
  setRefreshCookie(event, response.refreshToken)

  return toClientResponse(response)
})
