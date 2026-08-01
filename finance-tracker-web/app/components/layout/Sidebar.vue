<script setup lang="ts">
const links = [
  { to: '/dashboard', label: 'Dashboard' },
  { to: '/dashboard/transactions', label: 'Transações' },
  { to: '/dashboard/categories', label: 'Categorias' },
]

const route = useRoute()
</script>

<template>
  <aside class="hidden w-56 shrink-0 border-r bg-background md:flex md:flex-col">
    <div class="flex h-14 items-center border-b px-4 text-lg font-semibold">
      Finance Tracker
    </div>
    <nav class="flex flex-col gap-1 p-2">
      <NuxtLink
        v-for="link in links"
        :key="link.to"
        :to="link.to"
        class="rounded-md px-3 py-2 text-sm font-medium transition-colors hover:bg-muted"
        :class="route.path === link.to ? 'bg-muted text-foreground' : 'text-muted-foreground'"
      >
        {{ link.label }}
      </NuxtLink>
    </nav>
  </aside>
</template>
