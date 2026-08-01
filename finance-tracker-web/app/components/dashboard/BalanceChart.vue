<script setup lang="ts">
import {
  CategoryScale,
  Chart as ChartJS,
  Filler,
  Legend,
  LinearScale,
  LineElement,
  PointElement,
  Tooltip,
} from 'chart.js'
import { computed } from 'vue'
import { Line } from 'vue-chartjs'
import { Card, CardContent, CardHeader, CardTitle } from '~/components/ui/card'
import type { BalanceHistoryPoint } from '~/types/finance'

ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Tooltip, Legend, Filler)

const props = defineProps<{ data: BalanceHistoryPoint[] }>()

const MONTH_LABELS = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez']

const chartData = computed(() => ({
  labels: props.data.map(d => `${MONTH_LABELS[d.month - 1]}/${String(d.year).slice(2)}`),
  datasets: [
    {
      label: 'Saldo',
      data: props.data.map(d => d.balance),
      borderColor: '#2563eb',
      backgroundColor: '#2563eb33',
      tension: 0.3,
      fill: true,
    },
  ],
}))

const chartOptions = {
  responsive: true,
  plugins: { legend: { display: false } },
}
</script>

<template>
  <Card>
    <CardHeader>
      <CardTitle>Evolução do saldo</CardTitle>
    </CardHeader>
    <CardContent>
      <Line :data="chartData" :options="chartOptions" />
    </CardContent>
  </Card>
</template>
