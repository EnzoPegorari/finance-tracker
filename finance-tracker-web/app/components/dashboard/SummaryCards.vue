<script setup lang="ts">
import { TrendingDown, TrendingUp, Wallet } from '@lucide/vue'
import { Card, CardContent, CardHeader, CardTitle } from '~/components/ui/card'
import type { DashboardSummary } from '~/types/finance'

defineProps<{ summary: DashboardSummary | null }>()

function formatCurrency(value: number) {
  return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}
</script>

<template>
  <div class="grid gap-4 sm:grid-cols-3">
    <Card>
      <CardHeader class="flex items-center justify-between pb-2">
        <CardTitle class="text-sm font-medium text-muted-foreground">
          Saldo atual
        </CardTitle>
        <Wallet class="size-4 text-cyan-400" />
      </CardHeader>
      <CardContent>
        <p
          class="text-2xl font-semibold"
          :class="(summary?.balance ?? 0) < 0 ? 'text-destructive' : 'text-gradient-brand'"
        >
          {{ summary ? formatCurrency(summary.balance) : '—' }}
        </p>
      </CardContent>
    </Card>
    <Card>
      <CardHeader class="flex items-center justify-between pb-2">
        <CardTitle class="text-sm font-medium text-muted-foreground">
          Receitas do mês
        </CardTitle>
        <TrendingUp class="size-4 text-emerald-400" />
      </CardHeader>
      <CardContent>
        <p class="text-2xl font-semibold text-emerald-400">
          {{ summary ? formatCurrency(summary.monthlyIncome) : '—' }}
        </p>
      </CardContent>
    </Card>
    <Card>
      <CardHeader class="flex items-center justify-between pb-2">
        <CardTitle class="text-sm font-medium text-muted-foreground">
          Despesas do mês
        </CardTitle>
        <TrendingDown class="size-4 text-destructive" />
      </CardHeader>
      <CardContent>
        <p class="text-2xl font-semibold text-destructive">
          {{ summary ? formatCurrency(summary.monthlyExpense) : '—' }}
        </p>
      </CardContent>
    </Card>
  </div>
</template>
