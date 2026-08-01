<script setup lang="ts">
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
      <CardHeader class="pb-2">
        <CardTitle class="text-sm font-medium text-muted-foreground">
          Saldo atual
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p class="text-2xl font-semibold" :class="(summary?.balance ?? 0) < 0 ? 'text-destructive' : 'text-foreground'">
          {{ summary ? formatCurrency(summary.balance) : '—' }}
        </p>
      </CardContent>
    </Card>
    <Card>
      <CardHeader class="pb-2">
        <CardTitle class="text-sm font-medium text-muted-foreground">
          Receitas do mês
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p class="text-2xl font-semibold text-emerald-600">
          {{ summary ? formatCurrency(summary.monthlyIncome) : '—' }}
        </p>
      </CardContent>
    </Card>
    <Card>
      <CardHeader class="pb-2">
        <CardTitle class="text-sm font-medium text-muted-foreground">
          Despesas do mês
        </CardTitle>
      </CardHeader>
      <CardContent>
        <p class="text-2xl font-semibold text-destructive">
          {{ summary ? formatCurrency(summary.monthlyExpense) : '—' }}
        </p>
      </CardContent>
    </Card>
  </div>
</template>
