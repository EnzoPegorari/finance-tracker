<script setup lang="ts">
import { Button } from '~/components/ui/button'
import TransactionFilters from '~/components/transactions/TransactionFilters.vue'
import TransactionForm from '~/components/transactions/TransactionForm.vue'
import TransactionList from '~/components/transactions/TransactionList.vue'
import type { CreateTransactionPayload } from '~/composables/useTransactions'
import type { CategoryDto, TransactionDto } from '~/types/finance'

definePageMeta({ layout: 'dashboard', middleware: 'auth' })

const { list, create, update, remove, exportCsv } = useTransactions()
const { list: listCategories } = useCategories()

const categories = ref<CategoryDto[]>([])
const transactions = ref<TransactionDto[]>([])
const totalCount = ref(0)
const page = ref(1)
const pageSize = 10

const from = ref('')
const to = ref('')
const categoryId = ref('all')
const type = ref('all')

const isFormOpen = ref(false)
const editingTransaction = ref<TransactionDto | null>(null)

const totalPages = computed(() => Math.max(1, Math.ceil(totalCount.value / pageSize)))

async function loadTransactions() {
  const result = await list({
    from: from.value || undefined,
    to: to.value || undefined,
    categoryId: categoryId.value !== 'all' ? categoryId.value : undefined,
    type: type.value !== 'all' ? (type.value as 'income' | 'expense') : undefined,
    page: page.value,
    pageSize,
  })
  transactions.value = result.items
  totalCount.value = result.totalCount
}

async function loadCategories() {
  categories.value = await listCategories()
}

function clearFilters() {
  from.value = ''
  to.value = ''
  categoryId.value = 'all'
  type.value = 'all'
  page.value = 1
}

watch([from, to, categoryId, type], () => {
  page.value = 1
  loadTransactions()
})

watch(page, loadTransactions)

function openCreateForm() {
  editingTransaction.value = null
  isFormOpen.value = true
}

function openEditForm(transaction: TransactionDto) {
  editingTransaction.value = transaction
  isFormOpen.value = true
}

async function onSubmit(payload: CreateTransactionPayload) {
  if (editingTransaction.value) {
    await update(editingTransaction.value.id, payload)
  }
  else {
    await create(payload)
  }
  isFormOpen.value = false
  await loadTransactions()
}

async function onDelete(transaction: TransactionDto) {
  if (!confirm(`Excluir a transação "${transaction.description}"?`))
    return

  await remove(transaction.id)
  await loadTransactions()
}

onMounted(async () => {
  await Promise.all([loadCategories(), loadTransactions()])
})
</script>

<template>
  <div class="flex flex-col gap-6">
    <div class="flex items-center justify-between">
      <h1 class="text-2xl font-semibold">
        Transações
      </h1>
      <div class="flex gap-2">
        <Button variant="outline" @click="exportCsv(from || undefined, to || undefined)">
          Exportar CSV
        </Button>
        <Button @click="openCreateForm">
          Nova transação
        </Button>
      </div>
    </div>

    <TransactionFilters
      v-model:from="from"
      v-model:to="to"
      v-model:category-id="categoryId"
      v-model:type="type"
      :categories="categories"
      @clear="clearFilters"
    />

    <div class="rounded-lg border bg-background">
      <TransactionList :transactions="transactions" @edit="openEditForm" @delete="onDelete" />
    </div>

    <div v-if="totalPages > 1" class="flex items-center justify-center gap-3">
      <Button variant="outline" size="sm" :disabled="page <= 1" @click="page--">
        Anterior
      </Button>
      <span class="text-sm text-muted-foreground">Página {{ page }} de {{ totalPages }}</span>
      <Button variant="outline" size="sm" :disabled="page >= totalPages" @click="page++">
        Próxima
      </Button>
    </div>

    <TransactionForm
      v-model:open="isFormOpen"
      :categories="categories"
      :editing-transaction="editingTransaction"
      @submit="onSubmit"
    />
  </div>
</template>
