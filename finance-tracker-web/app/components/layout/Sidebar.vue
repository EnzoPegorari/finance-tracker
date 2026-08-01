<script setup lang="ts">
import { ArrowLeftRight, LayoutDashboard, Tags, Zap } from '@lucide/vue'

const links = [
  { to: '/dashboard', label: 'Dashboard', icon: LayoutDashboard },
  { to: '/dashboard/transactions', label: 'Transactions', icon: ArrowLeftRight },
  { to: '/dashboard/categories', label: 'Categories', icon: Tags },
]

const route = useRoute()
</script>

<template>
  <aside class="hidden w-60 shrink-0 border-r border-sidebar-border bg-sidebar md:flex md:flex-col">
    <div class="flex h-14 items-center gap-2 border-b border-sidebar-border px-4">
      <span class="flex size-7 items-center justify-center rounded-lg bg-gradient-to-br from-cyan-400 to-blue-600 shadow-[0_0_14px_-2px_theme(colors.cyan.400)]">
        <Zap class="size-4 text-black" />
      </span>
      <span class="text-lg font-semibold text-gradient-brand">Finance Tracker</span>
    </div>
    <nav class="flex flex-col gap-1 p-3">
      <NuxtLink
        v-for="link in links"
        :key="link.to"
        :to="link.to"
        class="group flex items-center gap-2.5 rounded-lg px-3 py-2 text-sm font-medium transition-all duration-200"
        :class="route.path === link.to
          ? 'bg-gradient-to-r from-cyan-500/15 to-transparent text-cyan-300 shadow-[inset_1px_0_0_0_theme(colors.cyan.400)]'
          : 'text-muted-foreground hover:bg-accent hover:text-accent-foreground'"
      >
        <component
          :is="link.icon"
          class="size-4 shrink-0 transition-colors"
          :class="route.path === link.to ? 'text-cyan-300' : 'text-muted-foreground group-hover:text-cyan-300'"
        />
        {{ link.label }}
      </NuxtLink>
    </nav>
  </aside>
</template>
