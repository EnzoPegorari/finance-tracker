import tailwindcss from '@tailwindcss/vite'

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  modules: ['@pinia/nuxt'],
  css: ['~/assets/css/tailwind.css'],
  vite: {
    plugins: [tailwindcss()],
  },
  runtimeConfig: {
    apiBase: process.env.NUXT_API_BASE || 'http://localhost:5299/api/v1',
    public: {
      apiBase: process.env.NUXT_PUBLIC_API_BASE || 'http://localhost:5299/api/v1',
    },
  },
})
