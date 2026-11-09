import { Link, NavLink, Outlet, useNavigate } from 'react-router-dom'
import useAuth from '../../hooks/useAuth'

const navigationItems = [
  { label: 'Dashboard', icon: '🏠', to: '/admin', end: true },
  { label: 'Characters', icon: '👤', to: '/admin/characters' },
  { label: 'Episodes', icon: '📺', to: '/admin/episodes' },
  { label: 'Locations', icon: '📍', to: '/admin/locations' },
]

function AdminLayout() {
  const navigate = useNavigate()
  const { logout, user } = useAuth()

  function handleLogout() {
    logout()
    navigate('/', { replace: true })
  }

  return (
    <div className="container-fluid py-4">
      <div className="row g-4">
        <aside className="col-12 col-lg-3 col-xl-2">
          <div className="card shadow-sm">
            <div className="card-body">
              <NavLink className="d-block h5 text-decoration-none mb-1" to="/admin">
                Big Bang Admin
              </NavLink>
              <p className="text-body-secondary small mb-4">
                {user?.userName || 'Administrador'}
              </p>

              <nav aria-label="Navegação administrativa" className="nav nav-pills flex-column gap-2">
                {navigationItems.map((item) => (
                  <NavLink
                    className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
                    end={item.end}
                    key={item.to}
                    to={item.to}
                  >
                    <span aria-hidden="true" className="me-2">{item.icon}</span>
                    {item.label}
                  </NavLink>
                ))}
              </nav>

              <div className="d-grid gap-2 mt-4">
                <Link className="btn btn-outline-secondary" to="/">
                  <span aria-hidden="true" className="me-2">🏡</span>
                  Voltar ao Site
                </Link>
                <button className="btn btn-outline-danger" onClick={handleLogout} type="button">
                  <span aria-hidden="true" className="me-2">🚪</span>
                  Logout
                </button>
              </div>
            </div>
          </div>
        </aside>

        <main className="col-12 col-lg-9 col-xl-10">
          <Outlet />
        </main>
      </div>
    </div>
  )
}

export default AdminLayout
