import { Navigate, Outlet } from 'react-router-dom'
import useAuth from '../hooks/useAuth'

function AdminRoute() {
  const { isAuthenticated, isAdmin } = useAuth()

  if (!isAuthenticated) {
    return <Navigate replace to="/login" />
  }

  if (!isAdmin) {
    return <Navigate replace to="/" />
  }

  return <Outlet />
}

export default AdminRoute
