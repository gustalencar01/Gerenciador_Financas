import { Routes, Route } from 'react-router-dom'
import Sidebar from './Sidebar.jsx'
import Dashboard from '../pages/Dashboard.jsx'
import Receitas from '../pages/Receitas.jsx'
import Despesas from '../pages/Despesas.jsx'
import Limites from '../pages/Limites.jsx'

export default function App() {
  return (
    <div className="flex min-h-screen bg-slate-50">
      <Sidebar />
      <main className="flex-1 ml-64 p-8">
        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/receitas" element={<Receitas />} />
          <Route path="/despesas" element={<Despesas />} />
          <Route path="/limites" element={<Limites />} />
        </Routes>
      </main>
    </div>
  )
}