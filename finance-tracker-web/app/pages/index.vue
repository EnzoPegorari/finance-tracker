<script setup lang="ts">
import { useAuthStore } from '~/stores/auth'

const authStore = useAuthStore()

const restored = await authStore.refresh()
await navigateTo(restored ? '/dashboard' : '/login')
</script>

<template>
  <div />
</template>
