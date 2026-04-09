import { NavLink } from 'react-router-dom'

const navItems = [
  { label: 'Dashboard', path: '/' },
  { label: 'Receitas', path: '/receitas' },
  { label: 'Despesas', path: '/despesas' },
  { label: 'Limites', path: '/limites' },
]

export default function Sidebar() {
  return (
    <aside className="fixed left-0 top-0 h-full w-64 border-r border-slate-200 bg-white px-6 py-8 shadow-sm">
      <div className="mb-12">
        <p className="text-sm uppercase tracking-[0.32em] text-slate-500">Financial Manager</p>
        <h1 className="mt-3 text-3xl font-semibold text-slate-950">Finanças</h1>
      </div>

      <nav className="space-y-2">
        {navItems.map((item) => (
          <NavLink
            key={item.path}
            to={item.path}
            className={({ isActive }) =>
              `block rounded-3xl px-4 py-3 text-sm font-medium transition ${
                isActive ? 'bg-slate-950 text-white' : 'text-slate-700 hover:bg-slate-100'
              }`
            }
          >
            {item.label}
          </NavLink>
        ))}
      </nav>
    </aside>
  )
}
