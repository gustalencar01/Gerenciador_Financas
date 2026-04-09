import { createContext, useCallback, useContext, useMemo, useState } from 'react'

const ToastContext = createContext(null)

export function ToastProvider({ children }) {
  const [toast, setToast] = useState(null)

  const showToast = useCallback((message, type = 'success') => {
    setToast({ message, type })
    window.setTimeout(() => setToast(null), 3500)
  }, [])

  const closeToast = useCallback(() => setToast(null), [])

  const value = useMemo(() => ({ showToast, closeToast }), [showToast, closeToast])

  return (
    <ToastContext.Provider value={value}>
      {children}
      {toast ? (
        <div className="fixed bottom-6 right-6 z-50 rounded-2xl border border-slate-200 bg-slate-950 px-5 py-4 text-sm text-white shadow-2xl shadow-slate-900/10">
          <div className="flex items-center gap-3">
            <span className="font-semibold uppercase tracking-[0.16em] text-slate-300">
              {toast.type}
            </span>
            <span>{toast.message}</span>
            <button type="button" onClick={closeToast} className="ml-auto text-slate-400 hover:text-white">
              Fechar
            </button>
          </div>
        </div>
      ) : null}
    </ToastContext.Provider>
  )
}

export function useToast() {
  const context = useContext(ToastContext)
  if (!context) {
    throw new Error('useToast deve ser usado dentro de ToastProvider')
  }
  return context
}
