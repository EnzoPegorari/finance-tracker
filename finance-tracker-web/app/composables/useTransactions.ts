import { apiFetch } from '~/services/api'
import { useAuthStore } from '~/stores/auth'
import type { PagedResult, TransactionDto, TransactionFilter } from '~/types/finance'

export interface CreateTransactionPayload {
  description: string
  amount: number
  type: 'income' | 'expense'
  date: string
  categoryId: string
  notes?: string | null
}

export function useTransactions() {
  const list = (filter: TransactionFilter = {}) =>
    apiFetch<PagedResult<TransactionDto>>('/transactions', { query: filter })

  const create = (payload: CreateTransactionPayload) =>
    apiFetch<TransactionDto>('/transactions', { method: 'POST', body: payload })

  const update = (id: string, payload: CreateTransactionPayload) =>
    apiFetch<TransactionDto>(`/transactions/${id}`, { method: 'PUT', body: payload })

  const remove = (id: string) =>
    apiFetch<void>(`/transactions/${id}`, { method: 'DELETE' })

  const exportCsv = async (from?: string, to?: string) => {
    const config = useRuntimeConfig()
    const authStore = useAuthStore()
    const query = new URLSearchParams()
    if (from) query.set('from', from)
    if (to) query.set('to', to)

    const response = await fetch(`${config.public.apiBase}/transactions/export?${query}`, {
      headers: { Authorization: `Bearer ${authStore.accessToken}` },
    })
    const blob = await response.blob()
    const url = URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    link.download = 'transactions.csv'
    link.click()
    URL.revokeObjectURL(url)
  }

  return { list, create, update, remove, exportCsv }
}
