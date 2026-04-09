export function Button({ children, className = '', ...props }) {
  return (
    <button
      className={`rounded-xl bg-slate-900 px-4 py-2 text-sm font-semibold text-white transition hover:bg-slate-700 disabled:cursor-not-allowed disabled:opacity-60 ${className}`}
      {...props}
    >
      {children}
    </button>
  )
}

export function Input({ className = '', ...props }) {
  return (
    <input
      className={`w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-sm text-slate-900 shadow-sm outline-none transition focus:border-slate-900 focus:ring-2 focus:ring-slate-200 ${className}`}
      {...props}
    />
  )
}

export function Select({ className = '', children, ...props }) {
  return (
    <select
      className={`w-full rounded-xl border border-slate-300 bg-white px-4 py-3 text-sm text-slate-900 shadow-sm outline-none transition focus:border-slate-900 focus:ring-2 focus:ring-slate-200 ${className}`}
      {...props}
    >
      {children}
    </select>
  )
}

export function Card({ className = '', children, ...props }) {
  return (
    <section className={`rounded-3xl border border-slate-200 bg-white p-6 shadow-sm ${className}`} {...props}>
      {children}
    </section>
  )
}

export function Badge({ className = '', children, ...props }) {
  return (
    <span className={`inline-flex rounded-full bg-slate-100 px-3 py-1 text-xs font-semibold uppercase tracking-[0.18em] text-slate-700 ${className}`} {...props}>
      {children}
    </span>
  )
}

export function Modal({ title, open, onClose, children }) {
  if (!open) return null

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-slate-950/60 p-4">
      <div className="w-full max-w-2xl overflow-hidden rounded-[28px] bg-white p-6 shadow-2xl">
        <div className="mb-4 flex items-center justify-between">
          <h2 className="text-lg font-semibold text-slate-900">{title}</h2>
          <button className="text-slate-500 hover:text-slate-900" onClick={onClose}>
            Fechar
          </button>
        </div>
        {children}
      </div>
    </div>
  )
}
