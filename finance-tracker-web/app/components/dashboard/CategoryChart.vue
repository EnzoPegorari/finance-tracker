<script setup lang="ts">
import { ArcElement, Chart as ChartJS, Legend, Tooltip } from 'chart.js'
import { computed } from 'vue'
import { Pie } from 'vue-chartjs'
import { Card, CardContent, CardHeader, CardTitle } from '~/components/ui/card'
import type { CategoryBreakdown } from '~/types/finance'

ChartJS.register(ArcElement, Tooltip, Legend)

const props = defineProps<{ data: CategoryBreakdown[] }>()

const chartData = computed(() => ({
  labels: props.data.map(d => d.categoryName),
  datasets: [
    {
      data: props.data.map(d => d.total),
      backgroundColor: props.data.map(d => d.color),
    },
  ],
}))

const chartOptions = {
  responsive: true,
  plugins: { legend: { position: 'bottom' as const } },
}
</script>

<template>
  <Card>
    <CardHeader>
      <CardTitle>Gastos por categoria</CardTitle>
    </CardHeader>
    <CardContent>
      <p v-if="data.length === 0" class="py-8 text-center text-sm text-muted-foreground">
        Nenhuma despesa neste mês ainda.
      </p>
      <Pie v-else :data="chartData" :options="chartOptions" />
    </CardContent>
  </Card>
</template>
