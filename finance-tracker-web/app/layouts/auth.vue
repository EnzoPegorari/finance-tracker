<script setup lang="ts">
import { Zap } from '@lucide/vue'
</script>

<template>
  <div class="flex min-h-screen flex-col items-center justify-center p-4">
    <div class="mb-6 flex items-center gap-2.5">
      <span class="flex size-9 items-center justify-center rounded-xl bg-gradient-to-br from-cyan-400 to-blue-600 shadow-[0_0_20px_-2px_theme(colors.cyan.400)]">
        <Zap class="size-5 text-black" />
      </span>
      <span class="text-xl font-semibold text-gradient-brand">Finance Tracker</span>
    </div>
    <slot />
  </div>
</template>
