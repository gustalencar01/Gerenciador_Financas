import { api } from './axios'

export async function fetchDespesas() {
  const { data } = await api.get('/despesas')
  return data
}

export async function createDespesa(payload) {
  const { data } = await api.post('/despesas', payload)
  return data
}

export async function updateDespesa(id, payload) {
  const { data } = await api.put(`/despesas/${id}`, payload)
  return data
}

export async function deleteDespesa(id) {
  const { data } = await api.delete(`/despesas/${id}`)
  return data
}
