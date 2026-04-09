import { useCallback, useEffect, useMemo, useState } from 'react'
import { Card, Button, Input, Select } from '../components/ui/index.jsx'
import { useToast } from '../context/ToastContext.jsx'
import { CATEGORIAS, formatCurrency, formatDate } from '../utils/format.js'
import { createDespesa, deleteDespesa, fetchDespesas, updateDespesa } from '../api/despesas.js'

const initialForm = {
  descricao: '',
  valor: '',
  categoria: CATEGORIAS[0],
  data: new Date().toISOString().slice(0, 10),
  pago: false,
}

export default function Despesas() {
  const [despesas, setDespesas] = useState([])
  const [form, setForm] = useState(initialForm)
  const [editingId, setEditingId] = useState(null)
  const [statusLimite, setStatusLimite] = useState('')
  const [loading, setLoading] = useState(false)
  const { showToast } = useToast()

  const totalDespesas = useMemo(
    () => despesas.reduce((sum, despesa) => sum + Number(despesa.valor || 0), 0),
    [despesas],
  )

  const loadDespesas = useCallback(async () => {
    setLoading(true)
    try {
      const data = await fetchDespesas()
      setDespesas(Array.isArray(data) ? data : [])
    } catch {
      showToast('Erro ao buscar despesas.', 'error')
    } finally {
      setLoading(false)
    }
  }, [showToast])

  useEffect(() => {
    loadDespesas()
  }, [loadDespesas])

  function handleChange(event) {
    const { name, value, type, checked } = event.target
    setForm((current) => ({
      ...current,
      [name]: type === 'checkbox' ? checked : value,
    }))
  }

  async function handleSubmit(event) {
    event.preventDefault()
    setLoading(true)
    const payload = {
      descricao: form.descricao,
      valor: Number(form.valor),
      categoria: form.categoria,
      data: form.data,
      pago: form.pago,
    }

    try {
      if (editingId) {
        const response = await updateDespesa(editingId, payload)
        showToast(response?.mensagem ?? 'Despesa atualizada com sucesso.', 'success')
      } else {
        const response = await createDespesa(payload)
        showToast(response?.mensagem ?? 'Despesa criada com sucesso.', 'success')
        setStatusLimite(response?.statusLimite ?? '')
      }

      setForm(initialForm)
      setEditingId(null)
      await loadDespesas()
    } catch {
      showToast('Erro ao salvar despesa.', 'error')
    } finally {
      setLoading(false)
    }
  }

  async function handleDelete(id) {
    if (!window.confirm('Deseja realmente excluir esta despesa?')) {
      return
    }

    setLoading(true)
    try {
      const response = await deleteDespesa(id)
      showToast(response?.mensagem ?? 'Despesa removida com sucesso.', 'success')
      await loadDespesas()
    } catch {
      showToast('Erro ao excluir despesa.', 'error')
    } finally {
      setLoading(false)
    }
  }

  function handleEdit(item) {
    setEditingId(item.id)
    setForm({
      descricao: item.descricao || '',
      valor: item.valor?.toString() || '',
      categoria: item.categoria || CATEGORIAS[0],
      data: item.data ? item.data.slice(0, 10) : new Date().toISOString().slice(0, 10),
      pago: Boolean(item.pago),
    })
    setStatusLimite('')
  }

  function handleCancelEdit() {
    setEditingId(null)
    setForm(initialForm)
    setStatusLimite('')
  }

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-end md:justify-between">
        <div>
          <h2 className="text-3xl font-semibold text-slate-950">Despesas</h2>
          <p className="mt-2 text-sm text-slate-500">Cadastre, edite e exclua suas despesas com status do limite.</p>
        </div>
        <div className="rounded-3xl border border-slate-200 bg-white p-5 shadow-sm">
          <p className="text-sm text-slate-500">Total de despesas</p>
          <p className="mt-2 text-3xl font-semibold text-slate-950">{formatCurrency(totalDespesas)}</p>
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
                placeholder="Descrição da despesa"
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
                placeholder="0.00"
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

          <div className="flex items-center gap-4">
            <label className="inline-flex items-center gap-2 text-sm text-slate-700">
              <input
                type="checkbox"
                name="pago"
                checked={form.pago}
                onChange={handleChange}
                className="h-4 w-4 rounded border-slate-300 text-slate-950 focus:ring-slate-900"
              />
              Pago
            </label>
            <div className="ml-auto flex items-center gap-3">
              {editingId ? (
                <Button type="button" className="bg-slate-300 text-slate-800 hover:bg-slate-400" onClick={handleCancelEdit}>
                  Cancelar
                </Button>
              ) : null}
              <Button type="submit" disabled={loading}>
                {editingId ? 'Atualizar despesa' : 'Adicionar despesa'}
              </Button>
            </div>
          </div>

          {statusLimite ? (
            <div className="rounded-2xl border border-orange-200 bg-orange-50 px-4 py-3 text-sm text-orange-900">
              {statusLimite}
            </div>
          ) : null}
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
                <th className="px-4 py-3">Pago</th>
                <th className="px-4 py-3">Ações</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-200">
              {despesas.length === 0 ? (
                <tr>
                  <td colSpan="6" className="px-4 py-6 text-center text-slate-500">
                    {loading ? 'Carregando despesas...' : 'Nenhuma despesa encontrada.'}
                  </td>
                </tr>
              ) : (
                despesas.map((despesa) => (
                  <tr key={despesa.id} className="hover:bg-slate-50">
                    <td className="px-4 py-4">{despesa.descricao}</td>
                    <td className="px-4 py-4">{despesa.categoria}</td>
                    <td className="px-4 py-4 font-semibold text-slate-950">{formatCurrency(despesa.valor)}</td>
                    <td className="px-4 py-4">{formatDate(despesa.data)}</td>
                    <td className="px-4 py-4">{despesa.pago ? 'Sim' : 'Não'}</td>
                    <td className="px-4 py-4">
                      <div className="flex flex-wrap gap-2">
                        <Button type="button" className="bg-slate-900 hover:bg-slate-700" onClick={() => handleEdit(despesa)}>
                          Editar
                        </Button>
                        <Button type="button" className="bg-rose-600 hover:bg-rose-500" onClick={() => handleDelete(despesa.id)}>
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
