<script setup lang="ts">
import { Button } from '~/components/ui/button'
import { Dialog, DialogContent, DialogFooter, DialogHeader, DialogTitle } from '~/components/ui/dialog'
import { Input } from '~/components/ui/input'
import { Label } from '~/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '~/components/ui/select'
import type { CreateTransactionPayload } from '~/composables/useTransactions'
import type { CategoryDto, TransactionDto } from '~/types/finance'

const props = defineProps<{
  open: boolean
  categories: CategoryDto[]
  editingTransaction: TransactionDto | null
}>()

const emit = defineEmits<{
  (e: 'update:open', value: boolean): void
  (e: 'submit', payload: CreateTransactionPayload): void
}>()

const description = ref('')
const amount = ref<number | undefined>(undefined)
const type = ref<'income' | 'expense'>('expense')
const date = ref(new Date().toISOString().slice(0, 10))
const categoryId = ref('')
const notes = ref('')

watch(() => props.open, (isOpen) => {
  if (!isOpen)
    return

  const t = props.editingTransaction
  description.value = t?.description ?? ''
  amount.value = t?.amount ?? undefined
  type.value = t?.type ?? 'expense'
  date.value = t?.date ?? new Date().toISOString().slice(0, 10)
  categoryId.value = t?.categoryId ?? ''
  notes.value = t?.notes ?? ''
})

function onOpenChange(value: boolean) {
  emit('update:open', value)
}

function onSubmit() {
  if (!description.value || amount.value === undefined || !categoryId.value)
    return

  emit('submit', {
    description: description.value,
    amount: amount.value,
    type: type.value,
    date: date.value,
    categoryId: categoryId.value,
    notes: notes.value || null,
  })
}
</script>

<template>
  <Dialog :open="open" @update:open="onOpenChange">
    <DialogContent>
      <DialogHeader>
        <DialogTitle>{{ editingTransaction ? 'Editar transação' : 'Nova transação' }}</DialogTitle>
      </DialogHeader>
      <form class="flex flex-col gap-4" @submit.prevent="onSubmit">
        <div class="flex flex-col gap-1.5">
          <Label for="description">Descrição</Label>
          <Input id="description" v-model="description" required />
        </div>
        <div class="grid grid-cols-2 gap-3">
          <div class="flex flex-col gap-1.5">
            <Label for="amount">Valor</Label>
            <Input id="amount" v-model.number="amount" type="number" min="0.01" step="0.01" required />
          </div>
          <div class="flex flex-col gap-1.5">
            <Label for="date">Data</Label>
            <Input id="date" v-model="date" type="date" required />
          </div>
        </div>
        <div class="grid grid-cols-2 gap-3">
          <div class="flex flex-col gap-1.5">
            <Label>Tipo</Label>
            <Select v-model="type">
              <SelectTrigger>
                <SelectValue />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="expense">
                  Despesa
                </SelectItem>
                <SelectItem value="income">
                  Receita
                </SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div class="flex flex-col gap-1.5">
            <Label>Categoria</Label>
            <Select v-model="categoryId">
              <SelectTrigger>
                <SelectValue placeholder="Selecione" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem v-for="category in categories" :key="category.id" :value="category.id">
                  {{ category.name }}
                </SelectItem>
              </SelectContent>
            </Select>
          </div>
        </div>
        <div class="flex flex-col gap-1.5">
          <Label for="notes">Notas (opcional)</Label>
          <Input id="notes" v-model="notes" />
        </div>
        <DialogFooter>
          <Button type="submit">
            {{ editingTransaction ? 'Salvar' : 'Criar' }}
          </Button>
        </DialogFooter>
      </form>
    </DialogContent>
  </Dialog>
</template>
