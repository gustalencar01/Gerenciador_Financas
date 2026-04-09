import { useCallback, useEffect, useMemo, useState } from 'react'
import { Card, Button, Input, Select } from '../components/ui/index.jsx'
import { useToast } from '../context/ToastContext.jsx'
import { CATEGORIAS, formatCurrency, formatDate } from '../utils/format.js'
import { createReceita, deleteReceita, fetchReceitas, updateReceita } from '../api/receitas.js'

const initialForm = {
  descricao: '',
  valor: '',
  categoria: CATEGORIAS[0],
  data: new Date().toISOString().slice(0, 10),
}

export default function Receitas() {
  const [receitas, setReceitas] = useState([])
  const [form, setForm] = useState(initialForm)
  const [editingId, setEditingId] = useState(null)
  const [loading, setLoading] = useState(false)
  const { showToast } = useToast()

  const totalReceitas = useMemo(
    () => receitas.reduce((sum, receita) => sum + Number(receita.valor || 0), 0),
    [receitas],
  )

  const loadReceitas = useCallback(async () => {
    setLoading(true)
    try {
      const data = await fetchReceitas()
      setReceitas(Array.isArray(data) ? data : [])
    } catch {
      showToast('Erro ao carregar receitas.', 'error')
    } finally {
      setLoading(false)
    }
  }, [showToast])

  useEffect(() => {
    loadReceitas()
  }, [loadReceitas])

  function handleChange(event) {
    const { name, value } = event.target
    setForm((current) => ({ ...current, [name]: value }))
  }

  async function handleSubmit(event) {
    event.preventDefault()
    setLoading(true)
    const payload = {
      descricao: form.descricao,
      valor: Number(form.valor),
      categoria: form.categoria,
      data: form.data,
    }

    try {
      if (editingId) {
        const response = await updateReceita(editingId, payload)
        showToast(response?.mensagem ?? 'Receita atualizada com sucesso.', 'success')
      } else {
        const response = await createReceita(payload)
        showToast(response?.mensagem ?? 'Receita adicionada com sucesso.', 'success')
      }

      setForm(initialForm)
      setEditingId(null)
      await loadReceitas()
    } catch {
      showToast('Erro ao salvar receita.', 'error')
    } finally {
      setLoading(false)
    }
  }

  async function handleDelete(id) {
    if (!window.confirm('Deseja excluir esta receita?')) return

    setLoading(true)
    try {
      const response = await deleteReceita(id)
      showToast(response?.mensagem ?? 'Receita excluída com sucesso.', 'success')
      await loadReceitas()
    } catch {
      showToast('Erro ao excluir receita.', 'error')
    } finally {
      setLoading(false)
    }
  }

  function handleEdit(receita) {
    setEditingId(receita.id)
    setForm({
      descricao: receita.descricao || '',
      valor: receita.valor?.toString() || '',
      categoria: receita.categoria || CATEGORIAS[0],
      data: receita.data ? receita.data.slice(0, 10) : new Date().toISOString().slice(0, 10),
    })
  }

  function handleCancel() {
    setEditingId(null)
    setForm(initialForm)
  }

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-end md:justify-between">
        <div>
          <h2 className="text-3xl font-semibold text-slate-950">Receitas</h2>
          <p className="mt-2 text-sm text-slate-500">Cadastre entradas e acompanhe o fluxo de receitas.</p>
        </div>
        <div className="rounded-3xl border border-slate-200 bg-white p-5 shadow-sm">
          <p className="text-sm text-slate-500">Total de receitas</p>
          <p className="mt-2 text-3xl font-semibold text-slate-950">{formatCurrency(totalReceitas)}</p>
        </div>
      </div>

      <Card>
        <form className="space-y-5" onSubmit={handleSubmit}>
          <div className="grid gap-4 md:grid-cols-2">
            <div>
              <label className="mb-2 block text-sm font-medium text-slate-700">Descrição</label>
              <Input
                name="descricao"
                value={form.descricao}
                onChange={handleChange}
                placeholder="Descrição da receita"
                required
              />
            </div>
            <div>
              <label className="mb-2 block text-sm font-medium text-slate-700">Categoria</label>
              <Select name="categoria" value={form.categoria} onChange={handleChange} required>
                {CATEGORIAS.map((categoria) => (
                  <option key={categoria} value={categoria}>
                    {categoria}
                  </option>
                ))}
              </Select>
            </div>
            <div>
              <label className="mb-2 block text-sm font-medium text-slate-700">Valor</label>
              <Input
                name="valor"
                type="number"
                value={form.valor}
                onChange={handleChange}
                min="0"
                step="0.01"
                required
              />
            </div>
            <div>
              <label className="mb-2 block text-sm font-medium text-slate-700">Data</label>
              <Input name="data" type="date" value={form.data} onChange={handleChange} required />
            </div>
          </div>

          <div className="flex items-center gap-3">
            {editingId ? (
              <Button
                type="button"
                className="bg-slate-300 text-slate-800 hover:bg-slate-400"
                onClick={handleCancel}
              >
                Cancelar
              </Button>
            ) : null}
            <Button type="submit" disabled={loading}>
              {editingId ? 'Atualizar receita' : 'Adicionar receita'}
            </Button>
          </div>
        </form>
      </Card>

      <Card>
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-slate-200 text-sm">
            <thead>
              <tr className="bg-slate-50 text-left text-slate-500">
                <th className="px-4 py-3">Descrição</th>
                <th className="px-4 py-3">Categoria</th>
                <th className="px-4 py-3">Valor</th>
                <th className="px-4 py-3">Data</th>
                <th className="px-4 py-3">Ações</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-200">
              {receitas.length === 0 ? (
                <tr>
                  <td colSpan="5" className="px-4 py-6 text-center text-slate-500">
                    {loading ? 'Carregando receitas...' : 'Nenhuma receita encontrada.'}
                  </td>
                </tr>
              ) : (
                receitas.map((receita) => (
                  <tr key={receita.id} className="hover:bg-slate-50">
                    <td className="px-4 py-4">{receita.descricao}</td>
                    <td className="px-4 py-4">{receita.categoria}</td>
                    <td className="px-4 py-4 font-semibold text-slate-950">{formatCurrency(receita.valor)}</td>
                    <td className="px-4 py-4">{formatDate(receita.data)}</td>
                    <td className="px-4 py-4">
                      <div className="flex flex-wrap gap-2">
                        <Button type="button" className="bg-slate-900 hover:bg-slate-700" onClick={() => handleEdit(receita)}>
                          Editar
                        </Button>
                        <Button type="button" className="bg-rose-600 hover:bg-rose-500" onClick={() => handleDelete(receita.id)}>
                          Excluir
                        </Button>
                      </div>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </Card>
    </div>
  )
}
