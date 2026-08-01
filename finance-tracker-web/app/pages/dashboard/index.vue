<script setup lang="ts">
import BalanceChart from '~/components/dashboard/BalanceChart.vue'
import CategoryChart from '~/components/dashboard/CategoryChart.vue'
import SummaryCards from '~/components/dashboard/SummaryCards.vue'
import type { BalanceHistoryPoint, CategoryBreakdown, DashboardSummary } from '~/types/finance'

definePageMeta({ layout: 'dashboard', middleware: 'auth' })

const { getSummary, getByCategory, getBalanceHistory } = useDashboard()

const summary = ref<DashboardSummary | null>(null)
const categoryBreakdown = ref<CategoryBreakdown[]>([])
const balanceHistory = ref<BalanceHistoryPoint[]>([])

const now = new Date()

async function loadDashboard() {
  const [summaryResult, categoryResult, historyResult] = await Promise.all([
    getSummary(),
    getByCategory(now.getMonth() + 1, now.getFullYear()),
    getBalanceHistory(6),
  ])
  summary.value = summaryResult
  categoryBreakdown.value = categoryResult
  balanceHistory.value = historyResult
}

onMounted(loadDashboard)
</script>

<template>
  <div class="flex flex-col gap-6">
    <h1 class="text-2xl font-semibold">
      Dashboard
    </h1>
    <SummaryCards :summary="summary" />
    <div class="grid gap-6 lg:grid-cols-2">
      <CategoryChart :data="categoryBreakdown" />
      <BalanceChart :data="balanceHistory" />
    </div>
  </div>
</template>
