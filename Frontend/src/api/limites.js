import { api } from './axios'

export async function fetchLimites() {
  const { data } = await api.get('/Limites')
  return data
}

export async function upsertLimite(payload) {
  const { data } = await api.post('/Limites', payload)
  return data
}

export async function deleteLimite(id) {
  const { data } = await api.delete(`/Limites/${id}`)
  return data
}
