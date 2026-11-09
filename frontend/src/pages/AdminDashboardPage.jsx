import useAuth from '../hooks/useAuth'

function AdminDashboardPage() {
  const { user } = useAuth()

  return (
    <section className="card shadow-sm">
      <div className="card-body p-4 p-md-5">
        <h1 className="h3 mb-3">Painel administrativo</h1>
        <p className="mb-0">
          Bem-vindo{user?.userName ? `, ${user.userName}` : ''} ao painel administrativo da Big Bang API.
        </p>
      </div>
    </section>
  )
}

export default AdminDashboardPage
