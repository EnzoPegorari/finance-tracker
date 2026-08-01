import { apiFetch } from '~/services/api'
import type { CategoryDto } from '~/types/finance'

export interface CategoryPayload {
  name: string
  color: string
  icon: string
}

export function useCategories() {
  const list = () => apiFetch<CategoryDto[]>('/categories')

  const create = (payload: CategoryPayload) =>
    apiFetch<CategoryDto>('/categories', { method: 'POST', body: payload })

  const update = (id: string, payload: CategoryPayload) =>
    apiFetch<CategoryDto>(`/categories/${id}`, { method: 'PUT', body: payload })

  const remove = (id: string) =>
    apiFetch<void>(`/categories/${id}`, { method: 'DELETE' })

  return { list, create, update, remove }
}
