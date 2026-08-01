<script setup lang="ts">
import { Button } from '~/components/ui/button'

const { user, logout } = useAuth()
const router = useRouter()

async function onLogout() {
  await logout()
  await router.push('/login')
}
</script>

<template>
  <header class="flex h-14 items-center justify-between border-b bg-background px-4">
    <span class="text-sm font-medium md:hidden">Finance Tracker</span>
    <div class="ml-auto flex items-center gap-3">
      <span class="text-sm text-muted-foreground">{{ user?.name }}</span>
      <Button variant="outline" size="sm" @click="onLogout">
        Sair
      </Button>
    </div>
  </header>
</template>
