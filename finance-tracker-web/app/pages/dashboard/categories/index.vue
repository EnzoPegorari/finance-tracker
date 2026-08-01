<script setup lang="ts">
import { Button } from '~/components/ui/button'
import { Card, CardContent } from '~/components/ui/card'
import { Dialog, DialogContent, DialogFooter, DialogHeader, DialogTitle } from '~/components/ui/dialog'
import { Input } from '~/components/ui/input'
import { Label } from '~/components/ui/label'
import type { CategoryPayload } from '~/composables/useCategories'
import type { CategoryDto } from '~/types/finance'

definePageMeta({ layout: 'dashboard', middleware: 'auth' })

const { list, create, update, remove } = useCategories()

const categories = ref<CategoryDto[]>([])
const isFormOpen = ref(false)
const editingCategory = ref<CategoryDto | null>(null)

const name = ref('')
const color = ref('#4F46E5')
const icon = ref('tag')

async function loadCategories() {
  categories.value = await list()
}

function openCreateForm() {
  editingCategory.value = null
  name.value = ''
  color.value = '#4F46E5'
  icon.value = 'tag'
  isFormOpen.value = true
}

function openEditForm(category: CategoryDto) {
  editingCategory.value = category
  name.value = category.name
  color.value = category.color
  icon.value = category.icon
  isFormOpen.value = true
}

async function onSubmit() {
  const payload: CategoryPayload = { name: name.value, color: color.value, icon: icon.value }

  if (editingCategory.value) {
    await update(editingCategory.value.id, payload)
  }
  else {
    await create(payload)
  }
  isFormOpen.value = false
  await loadCategories()
}

async function onDelete(category: CategoryDto) {
  if (!confirm(`Excluir a categoria "${category.name}"?`))
    return

  await remove(category.id)
  await loadCategories()
}

onMounted(loadCategories)
</script>

<template>
  <div class="flex flex-col gap-6">
    <div class="flex items-center justify-between">
      <h1 class="text-2xl font-semibold">
        Categorias
      </h1>
      <Button @click="openCreateForm">
        Nova categoria
      </Button>
    </div>

    <div class="grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
      <Card v-for="category in categories" :key="category.id">
        <CardContent class="flex items-center justify-between gap-3 py-4">
          <div class="flex items-center gap-2">
            <span class="size-3 rounded-full" :style="{ backgroundColor: category.color }" />
            <span class="font-medium">{{ category.name }}</span>
            <span v-if="category.isGlobal" class="rounded bg-muted px-1.5 py-0.5 text-xs text-muted-foreground">Padrão</span>
          </div>
          <div v-if="!category.isGlobal" class="flex gap-1">
            <Button variant="ghost" size="sm" @click="openEditForm(category)">
              Editar
            </Button>
            <Button variant="ghost" size="sm" class="text-destructive" @click="onDelete(category)">
              Excluir
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>

    <Dialog :open="isFormOpen" @update:open="isFormOpen = $event">
      <DialogContent>
        <DialogHeader>
          <DialogTitle>{{ editingCategory ? 'Editar categoria' : 'Nova categoria' }}</DialogTitle>
        </DialogHeader>
        <form class="flex flex-col gap-4" @submit.prevent="onSubmit">
          <div class="flex flex-col gap-1.5">
            <Label for="name">Nome</Label>
            <Input id="name" v-model="name" required />
          </div>
          <div class="grid grid-cols-2 gap-3">
            <div class="flex flex-col gap-1.5">
              <Label for="color">Cor</Label>
              <Input id="color" v-model="color" type="color" class="h-9 p-1" />
            </div>
            <div class="flex flex-col gap-1.5">
              <Label for="icon">Ícone (lucide)</Label>
              <Input id="icon" v-model="icon" placeholder="tag" />
            </div>
          </div>
          <DialogFooter>
            <Button type="submit">
              {{ editingCategory ? 'Salvar' : 'Criar' }}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  </div>
</template>
