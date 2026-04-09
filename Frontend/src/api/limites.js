import { api } from './axios'

export async function fetchLimites() {
  const { data } = await api.get('/limites')
  return data
}

export async function upsertLimite(payload) {
  const { data } = await api.post('/limites', payload)
  return data
}

export async function deleteLimite(id) {
  const { data } = await api.delete(`/limites/${id}`)
  return data
}
