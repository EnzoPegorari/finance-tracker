export default defineEventHandler(async (event) => {
  const body = await readBody(event)

  const response = await callBackendAuth(event, 'login', body)
  setRefreshCookie(event, response.refreshToken)

  return toClientResponse(response)
})
