import { useCallback, useEffect, useMemo, useState } from 'react'
import { Card } from '../components/ui/index.jsx'
import { formatCurrency } from '../utils/format.js'
import { fetchReceitas } from '../api/receitas.js'
import { fetchDespesas } from '../api/despesas.js'
import { fetchLimites } from '../api/limites.js'

export default function Dashboard() {
  const [receitas, setReceitas] = useState([])
  const [despesas, setDespesas] = useState([])
  const [limites, setLimites] = useState([])
  const [loading, setLoading] = useState(true)

  const { totalReceitas, totalDespesas, saldo } = useMemo(() => {
    const totalR = receitas.reduce((sum, r) => sum + Number(r.valor || 0), 0)
    const totalD = despesas.reduce((sum, d) => sum + Number(d.valor || 0), 0)
    return { totalReceitas: totalR, totalDespesas: totalD, saldo: totalR - totalD }
  }, [receitas, despesas])

  const loadData = useCallback(async () => {
    setLoading(true)
    try {
      const [receitasData, despesasData, limitesData] = await Promise.all([
        fetchReceitas().catch(() => []),
        fetchDespesas().catch(() => []),
        fetchLimites().catch(() => []),
      ])
      setReceitas(Array.isArray(receitasData) ? receitasData : [])
      setDespesas(Array.isArray(despesasData) ? despesasData : [])
      setLimites(Array.isArray(limitesData) ? limitesData : [])
    } catch {
      console.error('Erro ao carregar dados do dashboard')
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    loadData()
  }, [loadData])

  const receitasRecentes = receitas.slice(-5).reverse()
  const despesasRecentes = despesas.slice(-5).reverse()

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-sm uppercase tracking-[0.3em] text-slate-500">Bem-vindo</p>
          <h2 className="mt-2 text-3xl font-semibold text-slate-950">Resumo financeiro</h2>
        </div>
      </div>

      <div className="grid gap-6 xl:grid-cols-3">
        <Card>
          <p className="text-sm text-slate-500">Total de receitas</p>
          <p className="mt-3 text-3xl font-semibold text-emerald-600">{formatCurrency(totalReceitas)}</p>
          <p className="mt-2 text-xs text-slate-400">{receitas.length} transações</p>
        </Card>
        <Card>
          <p className="text-sm text-slate-500">Total de despesas</p>
          <p className="mt-3 text-3xl font-semibold text-rose-600">{formatCurrency(totalDespesas)}</p>
          <p className="mt-2 text-xs text-slate-400">{despesas.length} transações</p>
        </Card>
        <Card>
          <p className="text-sm text-slate-500">Saldo</p>
          <p className={`mt-3 text-3xl font-semibold ${saldo >= 0 ? 'text-slate-950' : 'text-rose-600'}`}>
            {formatCurrency(saldo)}
          </p>
          <p className="mt-2 text-xs text-slate-400">{saldo >= 0 ? 'Positivo' : 'Negativo'}</p>
        </Card>
      </div>

      <div className="grid gap-6 lg:grid-cols-2">
        <Card>
          <div className="mb-4 flex items-center justify-between">
            <h3 className="text-lg font-semibold text-slate-950">Receitas recentes</h3>
            {loading && <span className="text-xs text-slate-400">Carregando...</span>}
          </div>
          {receitasRecentes.length === 0 ? (
            <p className="text-sm text-slate-500">Nenhuma receita registrada</p>
          ) : (
            <div className="space-y-3">
              {receitasRecentes.map((receita) => (
                <div key={receita.id} className="flex items-center justify-between border-b border-slate-100 pb-3 last:border-0">
                  <div>
                    <p className="text-sm font-medium text-slate-950">{receita.descricao}</p>
                    <p className="text-xs text-slate-500">{receita.categoria}</p>
                  </div>
                  <p className="text-sm font-semibold text-emerald-600">+{formatCurrency(receita.valor)}</p>
                </div>
              ))}
            </div>
          )}
        </Card>

        <Card>
          <div className="mb-4 flex items-center justify-between">
            <h3 className="text-lg font-semibold text-slate-950">Despesas recentes</h3>
            {loading && <span className="text-xs text-slate-400">Carregando...</span>}
          </div>
          {despesasRecentes.length === 0 ? (
            <p className="text-sm text-slate-500">Nenhuma despesa registrada</p>
          ) : (
            <div className="space-y-3">
              {despesasRecentes.map((despesa) => (
                <div key={despesa.id} className="flex items-center justify-between border-b border-slate-100 pb-3 last:border-0">
                  <div>
                    <p className="text-sm font-medium text-slate-950">{despesa.descricao}</p>
                    <p className="text-xs text-slate-500">{despesa.categoria}</p>
                  </div>
                  <p className="text-sm font-semibold text-rose-600">-{formatCurrency(despesa.valor)}</p>
                </div>
              ))}
            </div>
          )}
        </Card>
      </div>

      <Card>
        <h3 className="mb-4 text-lg font-semibold text-slate-950">Limites configurados</h3>
        {limites.length === 0 ? (
          <p className="text-sm text-slate-500">Nenhum limite configurado. Configure limites por categoria.</p>
        ) : (
          <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
            {limites.map((limite) => (
              <div key={limite.id} className="rounded-2xl border border-slate-200 bg-slate-50 p-4">
                <p className="text-sm font-medium text-slate-700">{limite.categoria}</p>
                <p className="mt-2 text-2xl font-semibold text-slate-950">{formatCurrency(limite.valorLimite)}</p>
              </div>
            ))}
          </div>
        )}
      </Card>
    </div>
  )
}
