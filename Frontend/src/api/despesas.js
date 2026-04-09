import { api } from './axios'

export async function fetchDespesas() {
  const { data } = await api.get('/Despesas')
  return data
}

export async function createDespesa(payload) {
  const { data } = await api.post('/Despesas', payload)
  return data
}

export async function updateDespesa(id, payload) {
  const { data } = await api.put(`/Despesas/${id}`, payload)
  return data
}

export async function deleteDespesa(id) {
  const { data } = await api.delete(`/Despesas/${id}`)
  return data
}
