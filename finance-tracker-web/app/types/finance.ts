export type TransactionType = 'income' | 'expense'

export interface CategoryDto {
  id: string
  name: string
  color: string
  icon: string
  isGlobal: boolean
}

export interface TransactionDto {
  id: string
  description: string
  amount: number
  type: TransactionType
  date: string
  notes: string | null
  categoryId: string
  categoryName: string
  categoryColor: string
  categoryIcon: string
}

export interface PagedResult<T> {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
}

export interface TransactionFilter {
  from?: string
  to?: string
  categoryId?: string
  type?: TransactionType
  page?: number
  pageSize?: number
}

export interface DashboardSummary {
  balance: number
  monthlyIncome: number
  monthlyExpense: number
}

export interface CategoryBreakdown {
  categoryId: string
  categoryName: string
  color: string
  total: number
}

export interface BalanceHistoryPoint {
  year: number
  month: number
  income: number
  expense: number
  balance: number
}
