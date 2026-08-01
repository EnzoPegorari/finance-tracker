import { appendResponseHeader, getRequestHeader } from 'h3'
import { defineStore } from 'pinia'

export interface UserDto {
  id: string
  name: string
  email: string
}

interface AuthState {
  accessToken: string | null
  accessTokenExpiresAt: string | null
  user: UserDto | null
  refreshPromise: Promise<boolean> | null
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    accessToken: null,
    accessTokenExpiresAt: null,
    user: null,
    refreshPromise: null,
  }),

  getters: {
    isAuthenticated: state => !!state.accessToken,
  },

  actions: {
    setSession(payload: { accessToken: string, accessTokenExpiresAt: string, user: UserDto }) {
      this.accessToken = payload.accessToken
      this.accessTokenExpiresAt = payload.accessTokenExpiresAt
      this.user = payload.user
    },

    clearSession() {
      this.accessToken = null
      this.accessTokenExpiresAt = null
      this.user = null
    },

    async register(name: string, email: string, password: string) {
      const requestFetch = useRequestFetch()
      const response = await requestFetch('/api/auth/register', {
        method: 'POST',
        body: { name, email, password },
      })
      this.setSession(response)
    },

    async login(email: string, password: string) {
      const requestFetch = useRequestFetch()
      const response = await requestFetch('/api/auth/login', {
        method: 'POST',
        body: { email, password },
      })
      this.setSession(response)
    },

    async logout() {
      try {
        const requestFetch = useRequestFetch()
        await requestFetch('/api/auth/logout', { method: 'POST' })
      }
      finally {
        this.clearSession()
      }
    },

    /** Deduplicates concurrent refresh calls so parallel 401s only hit the BFF once. */
    async refresh(): Promise<boolean> {
      if (!this.refreshPromise) {
        this.refreshPromise = this.performRefresh().finally(() => {
          this.refreshPromise = null
        })
      }
      return this.refreshPromise
    },

    /**
     * Internal fetches to our own BFF routes run through a separate H3 event, so the
     * incoming cookie has to be forwarded manually and a rotated refresh cookie set on
     * that inner event has to be copied back onto the outer page response by hand.
     */
    async performRefresh(): Promise<boolean> {
      try {
        const event = import.meta.server ? useRequestEvent() : undefined
        const cookieHeader = event ? getRequestHeader(event, 'cookie') : undefined

        const rawResponse = await $fetch.raw('/api/auth/refresh', {
          method: 'POST',
          headers: cookieHeader ? { cookie: cookieHeader } : undefined,
        })

        if (event) {
          const setCookieHeader = rawResponse.headers.get('set-cookie')
          if (setCookieHeader) {
            appendResponseHeader(event, 'set-cookie', setCookieHeader)
          }
        }

        if (!rawResponse._data)
          throw new Error('Empty refresh response.')

        this.setSession(rawResponse._data)
        return true
      }
      catch {
        this.clearSession()
        return false
      }
    },
  },
})
