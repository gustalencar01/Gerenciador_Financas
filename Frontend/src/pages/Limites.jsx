import { useCallback, useEffect, useMemo, useState } from 'react'
import { Card, Button, Input, Select } from '../components/ui/index.jsx'
import { useToast } from '../context/ToastContext.jsx'
import { CATEGORIAS, formatCurrency } from '../utils/format.js'
import { deleteLimite, fetchLimites, upsertLimite } from '../api/limites.js'

const initialForm = {
  categoria: CATEGORIAS[0],
  valorLimite: '',
}

export default function Limites() {
  const [limites, setLimites] = useState([])
  const [form, setForm] = useState(initialForm)
  const [editingId, setEditingId] = useState(null)
  const [loading, setLoading] = useState(false)
  const { showToast } = useToast()

  const totalLimite = useMemo(
    () => limites.reduce((sum, limite) => sum + Number(limite.valorLimite || 0), 0),
    [limites],
  )

  const loadLimites = useCallback(async () => {
    setLoading(true)
    try {
      const data = await fetchLimites()
      setLimites(Array.isArray(data) ? data : [])
    } catch {
      showToast('Erro ao carregar limites.', 'error')
    } finally {
      setLoading(false)
    }
  }, [showToast])

  useEffect(() => {
    loadLimites()
  }, [loadLimites])

  function handleChange(event) {
    const { name, value } = event.target
    setForm((current) => ({ ...current, [name]: value }))
  }

  async function handleSubmit(event) {
    event.preventDefault()
    setLoading(true)
    const payload = {
      categoria: form.categoria,
      valorLimite: Number(form.valorLimite),
    }

    if (editingId) payload.id = editingId

    try {
      const response = await upsertLimite(payload)
      showToast(response?.mensagem ?? 'Limite salvo com sucesso.', 'success')
      setForm(initialForm)
      setEditingId(null)
      await loadLimites()
    } catch {
      showToast('Erro ao salvar limite.', 'error')
    } finally {
      setLoading(false)
    }
  }

  async function handleDelete(id) {
    if (!window.confirm('Deseja excluir este limite?')) return

    setLoading(true)
    try {
      const response = await deleteLimite(id)
      showToast(response?.mensagem ?? 'Limite excluído com sucesso.', 'success')
      await loadLimites()
    } catch {
      showToast('Erro ao excluir limite.', 'error')
    } finally {
      setLoading(false)
    }
  }

  function handleEdit(limite) {
    setEditingId(limite.id)
    setForm({
      categoria: limite.categoria || CATEGORIAS[0],
      valorLimite: limite.valorLimite?.toString() || '',
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
          <h2 className="text-3xl font-semibold text-slate-950">Limites</h2>
          <p className="mt-2 text-sm text-slate-500">Configure limites por categoria e atualize com um único POST.</p>
        </div>
        <div className="rounded-3xl border border-slate-200 bg-white p-5 shadow-sm">
          <p className="text-sm text-slate-500">Total de limite</p>
          <p className="mt-2 text-3xl font-semibold text-slate-950">{formatCurrency(totalLimite)}</p>
        </div>
      </div>

      <Card>
        <form className="space-y-5" onSubmit={handleSubmit}>
          <div className="grid gap-4 md:grid-cols-2">
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
              <label className="mb-2 block text-sm font-medium text-slate-700">Valor do limite</label>
              <Input
                name="valorLimite"
                type="number"
                value={form.valorLimite}
                onChange={handleChange}
                min="0"
                step="0.01"
                required
              />
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
              {editingId ? 'Atualizar limite' : 'Salvar limite'}
            </Button>
          </div>
        </form>
      </Card>

      <Card>
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-slate-200 text-sm">
            <thead>
              <tr className="bg-slate-50 text-left text-slate-500">
                <th className="px-4 py-3">Categoria</th>
                <th className="px-4 py-3">Valor limite</th>
                <th className="px-4 py-3">Ações</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-200">
              {limites.length === 0 ? (
                <tr>
                  <td colSpan="3" className="px-4 py-6 text-center text-slate-500">
                    {loading ? 'Carregando limites...' : 'Nenhum limite cadastrado.'}
                  </td>
                </tr>
              ) : (
                limites.map((limite) => (
                  <tr key={limite.id} className="hover:bg-slate-50">
                    <td className="px-4 py-4">{limite.categoria}</td>
                    <td className="px-4 py-4 font-semibold text-slate-950">{formatCurrency(limite.valorLimite)}</td>
                    <td className="px-4 py-4">
                      <div className="flex flex-wrap gap-2">
                        <Button type="button" className="bg-slate-900 hover:bg-slate-700" onClick={() => handleEdit(limite)}>
                          Editar
                        </Button>
                        <Button type="button" className="bg-rose-600 hover:bg-rose-500" onClick={() => handleDelete(limite.id)}>
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
