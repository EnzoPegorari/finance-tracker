<script setup lang="ts">
import { Button } from '~/components/ui/button'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '~/components/ui/table'
import type { TransactionDto } from '~/types/finance'

defineProps<{ transactions: TransactionDto[] }>()

const emit = defineEmits<{
  (e: 'edit', transaction: TransactionDto): void
  (e: 'delete', transaction: TransactionDto): void
}>()

function formatCurrency(value: number) {
  return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

function formatDate(value: string) {
  return new Date(`${value}T00:00:00`).toLocaleDateString('pt-BR')
}
</script>

<template>
  <Table>
    <TableHeader>
      <TableRow>
        <TableHead>Data</TableHead>
        <TableHead>Descrição</TableHead>
        <TableHead>Categoria</TableHead>
        <TableHead>Tipo</TableHead>
        <TableHead class="text-right">
          Valor
        </TableHead>
        <TableHead class="w-24" />
      </TableRow>
    </TableHeader>
    <TableBody>
      <TableRow v-if="transactions.length === 0">
        <TableCell colspan="6" class="text-center text-muted-foreground">
          Nenhuma transação encontrada.
        </TableCell>
      </TableRow>
      <TableRow v-for="transaction in transactions" :key="transaction.id">
        <TableCell>{{ formatDate(transaction.date) }}</TableCell>
        <TableCell>{{ transaction.description }}</TableCell>
        <TableCell>
          <span class="inline-flex items-center gap-1.5">
            <span
              class="size-2 rounded-full"
              :style="{ backgroundColor: transaction.categoryColor, boxShadow: `0 0 8px -1px ${transaction.categoryColor}` }"
            />
            {{ transaction.categoryName }}
          </span>
        </TableCell>
        <TableCell>
          <span
            class="rounded-full px-2 py-0.5 text-xs font-medium"
            :class="transaction.type === 'income' ? 'bg-emerald-400/10 text-emerald-400' : 'bg-destructive/10 text-destructive'"
          >
            {{ transaction.type === 'income' ? 'Receita' : 'Despesa' }}
          </span>
        </TableCell>
        <TableCell class="text-right font-medium" :class="transaction.type === 'income' ? 'text-emerald-400' : 'text-destructive'">
          {{ transaction.type === 'income' ? '+' : '-' }}{{ formatCurrency(transaction.amount) }}
        </TableCell>
        <TableCell class="flex justify-end gap-1">
          <Button variant="ghost" size="sm" @click="emit('edit', transaction)">
            Editar
          </Button>
          <Button variant="ghost" size="sm" class="text-destructive" @click="emit('delete', transaction)">
            Excluir
          </Button>
        </TableCell>
      </TableRow>
    </TableBody>
  </Table>
</template>
