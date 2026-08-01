import { apiFetch } from '~/services/api'
import type { BalanceHistoryPoint, CategoryBreakdown, DashboardSummary } from '~/types/finance'

export function useDashboard() {
  const getSummary = () => apiFetch<DashboardSummary>('/dashboard/summary')

  const getByCategory = (month: number, year: number) =>
    apiFetch<CategoryBreakdown[]>('/dashboard/by-category', { query: { month, year } })

  const getBalanceHistory = (months = 6) =>
    apiFetch<BalanceHistoryPoint[]>('/dashboard/balance-history', { query: { months } })

  return { getSummary, getByCategory, getBalanceHistory }
}
