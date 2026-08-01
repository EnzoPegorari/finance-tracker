<script setup lang="ts">
import { useVModel } from '@vueuse/core'
import { Button } from '~/components/ui/button'
import { Input } from '~/components/ui/input'
import { Label } from '~/components/ui/label'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '~/components/ui/select'
import type { CategoryDto } from '~/types/finance'

const props = defineProps<{
  categories: CategoryDto[]
  from: string
  to: string
  categoryId: string
  type: string
}>()

const emit = defineEmits<{
  (e: 'update:from', value: string): void
  (e: 'update:to', value: string): void
  (e: 'update:categoryId', value: string): void
  (e: 'update:type', value: string): void
  (e: 'clear'): void
}>()

const from = useVModel(props, 'from', emit)
const to = useVModel(props, 'to', emit)
const categoryId = useVModel(props, 'categoryId', emit)
const type = useVModel(props, 'type', emit)
</script>

<template>
  <div class="flex flex-wrap items-end gap-3 rounded-lg border bg-background p-4">
    <div class="flex flex-col gap-1.5">
      <Label for="filter-from">De</Label>
      <Input id="filter-from" v-model="from" type="date" class="w-40" />
    </div>
    <div class="flex flex-col gap-1.5">
      <Label for="filter-to">Até</Label>
      <Input id="filter-to" v-model="to" type="date" class="w-40" />
    </div>
    <div class="flex flex-col gap-1.5">
      <Label>Categoria</Label>
      <Select v-model="categoryId">
        <SelectTrigger class="w-44">
          <SelectValue placeholder="Todas" />
        </SelectTrigger>
        <SelectContent>
          <SelectItem value="all">
            Todas
          </SelectItem>
          <SelectItem v-for="category in categories" :key="category.id" :value="category.id">
            {{ category.name }}
          </SelectItem>
        </SelectContent>
      </Select>
    </div>
    <div class="flex flex-col gap-1.5">
      <Label>Tipo</Label>
      <Select v-model="type">
        <SelectTrigger class="w-36">
          <SelectValue placeholder="Todos" />
        </SelectTrigger>
        <SelectContent>
          <SelectItem value="all">
            Todos
          </SelectItem>
          <SelectItem value="income">
            Receita
          </SelectItem>
          <SelectItem value="expense">
            Despesa
          </SelectItem>
        </SelectContent>
      </Select>
    </div>
    <Button variant="outline" @click="emit('clear')">
      Limpar filtros
    </Button>
  </div>
</template>
