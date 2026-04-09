import { api } from './axios'

export async function fetchReceitas() {
  const { data } = await api.get('/Receitas')
  return data
}

export async function createReceita(payload) {
  const { data } = await api.post('/Receitas', payload)
  return data
}

export async function updateReceita(id, payload) {
  const { data } = await api.put(`/Receitas/${id}`, payload)
  return data
}

export async function deleteReceita(id) {
  const { data } = await api.delete(`/Receitas/${id}`)
  return data
}
