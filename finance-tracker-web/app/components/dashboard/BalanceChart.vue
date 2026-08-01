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

const MONTH_LABELS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']

function areaGradient(context: { chart: { ctx: CanvasRenderingContext2D, chartArea?: { top: number, bottom: number } } }) {
  const { ctx, chartArea } = context.chart
  if (!chartArea)
    return 'rgba(34, 211, 238, 0.25)'

  const gradient = ctx.createLinearGradient(0, chartArea.top, 0, chartArea.bottom)
  gradient.addColorStop(0, 'rgba(34, 211, 238, 0.35)')
  gradient.addColorStop(1, 'rgba(34, 211, 238, 0)')
  return gradient
}

const chartData = computed(() => ({
  labels: props.data.map(d => `${MONTH_LABELS[d.month - 1]}/${String(d.year).slice(2)}`),
  datasets: [
    {
      label: 'Balance',
      data: props.data.map(d => d.balance),
      borderColor: '#22d3ee',
      backgroundColor: areaGradient,
      pointBackgroundColor: '#22d3ee',
      pointBorderColor: '#0a0f1c',
      pointBorderWidth: 2,
      pointRadius: 4,
      pointHoverRadius: 6,
      borderWidth: 2.5,
      tension: 0.35,
      fill: true,
    },
  ],
}))

const chartOptions = {
  responsive: true,
  plugins: {
    legend: { display: false },
    tooltip: {
      backgroundColor: '#0a0f1c',
      titleColor: '#e2f8fc',
      bodyColor: '#67e8f9',
      borderColor: 'rgba(34, 211, 238, 0.3)',
      borderWidth: 1,
      padding: 10,
      cornerRadius: 8,
    },
  },
  scales: {
    x: {
      ticks: { color: '#94a3b8' },
      grid: { color: 'rgba(148, 163, 184, 0.08)' },
    },
    y: {
      ticks: { color: '#94a3b8' },
      grid: { color: 'rgba(148, 163, 184, 0.08)' },
    },
  },
}
</script>

<template>
  <Card>
    <CardHeader>
      <CardTitle>Balance evolution</CardTitle>
    </CardHeader>
    <CardContent>
      <Line :data="chartData" :options="chartOptions" />
    </CardContent>
  </Card>
</template>
