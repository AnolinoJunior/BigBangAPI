import { Navigate, Outlet } from 'react-router-dom'
import useAuth from '../hooks/useAuth'

function ProtectedRoute() {
  const { isAuthenticated } = useAuth()
  if (!isAuthenticated) {
    return <Navigate replace to="/login" />
  }

  return <Outlet />
}

export default ProtectedRoute
