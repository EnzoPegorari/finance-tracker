import type { H3Event } from 'h3'

export interface UserDto {
  id: string
  name: string
  email: string
}

export interface BackendAuthResponse {
  accessToken: string
  refreshToken: string
  accessTokenExpiresAt: string
  user: UserDto
}

export interface ClientAuthResponse {
  accessToken: string
  accessTokenExpiresAt: string
  user: UserDto
}

const REFRESH_COOKIE = 'refresh_token'
const REFRESH_COOKIE_MAX_AGE = 60 * 60 * 24 * 7 // 7 days, mirrors backend RefreshTokenExpirationDays

export async function callBackendAuth(event: H3Event, path: string, body: Record<string, unknown>): Promise<BackendAuthResponse> {
  const config = useRuntimeConfig(event)

  try {
    return await $fetch<BackendAuthResponse>(`${config.apiBase}/auth/${path}`, {
      method: 'POST',
      body,
    })
  }
  catch (error: any) {
    const message = error?.data?.error || 'Authentication request failed.'
    throw createError({ statusCode: error?.response?.status || 500, statusMessage: message })
  }
}

export function setRefreshCookie(event: H3Event, refreshToken: string) {
  setCookie(event, REFRESH_COOKIE, refreshToken, {
    httpOnly: true,
    secure: !import.meta.dev,
    sameSite: 'lax',
    path: '/',
    maxAge: REFRESH_COOKIE_MAX_AGE,
  })
}

export function clearRefreshCookie(event: H3Event) {
  deleteCookie(event, REFRESH_COOKIE, { path: '/' })
}

export function getRefreshCookie(event: H3Event): string | undefined {
  return getCookie(event, REFRESH_COOKIE)
}

export function toClientResponse(response: BackendAuthResponse): ClientAuthResponse {
  return {
    accessToken: response.accessToken,
    accessTokenExpiresAt: response.accessTokenExpiresAt,
    user: response.user,
  }
}
