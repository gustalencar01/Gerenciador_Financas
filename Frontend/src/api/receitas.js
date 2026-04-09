import { api } from './axios'

export async function fetchReceitas() {
  const { data } = await api.get('/receitas')
  return data
}

export async function createReceita(payload) {
  const { data } = await api.post('/receitas', payload)
  return data
}

export async function updateReceita(id, payload) {
  const { data } = await api.put(`/receitas/${id}`, payload)
  return data
}

export async function deleteReceita(id) {
  const { data } = await api.delete(`/receitas/${id}`)
  return data
}
