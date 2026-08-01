<script setup lang="ts">
import { LogOut } from '@lucide/vue'
import { Button } from '~/components/ui/button'

const { user, logout } = useAuth()
const router = useRouter()

const initials = computed(() =>
  (user.value?.name ?? '')
    .split(' ')
    .filter(Boolean)
    .slice(0, 2)
    .map(part => part[0]?.toUpperCase())
    .join(''),
)

async function onLogout() {
  await logout()
  await router.push('/login')
}
</script>

<template>
  <header class="flex h-14 items-center justify-between border-b border-border bg-background/70 px-4 backdrop-blur-md">
    <span class="text-sm font-medium text-gradient-brand md:hidden">Finance Tracker</span>
    <div class="ml-auto flex items-center gap-3">
      <div class="flex items-center gap-2">
        <span class="flex size-7 items-center justify-center rounded-full bg-gradient-to-br from-cyan-400 to-blue-600 text-xs font-semibold text-black">
          {{ initials }}
        </span>
        <span class="text-sm text-muted-foreground">{{ user?.name }}</span>
      </div>
      <Button variant="outline" size="sm" @click="onLogout">
        <LogOut class="size-3.5" />
        Sair
      </Button>
    </div>
  </header>
</template>
