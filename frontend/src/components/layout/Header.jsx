import { NavLink, useNavigate } from 'react-router-dom'
import useAuth from '../../hooks/useAuth'
import styles from './Header.module.css'

const navigationItems = [
  { label: 'Home', to: '/' },
  { label: 'Characters', to: '/characters' },
  { label: 'Episodes', to: '/episodes' },
  { label: 'Locations', to: '/locations' },
]

function Header() {
  const navigate = useNavigate()
  const { isAdmin, isAuthenticated, logout } = useAuth()

  function handleLogout() {
    logout()
    navigate('/', { replace: true })
  }

  return (
    <header className={styles.header}>
      <div className={styles.content}>
        <NavLink className={styles.brand} to="/">
          Big Bang API
        </NavLink>

        <nav aria-label="Navega\u00e7\u00e3o principal">
          <ul className={styles.navigation}>
            {navigationItems.map((item) => (
              <li key={item.to}>
                <NavLink
                  className={({ isActive }) =>
                    `${styles.link} ${isActive ? styles.active : ''}`
                  }
                  to={item.to}
                >
                  {item.label}
                </NavLink>
              </li>
            ))}
            {!isAuthenticated && (
              <li>
                <NavLink
                  className={({ isActive }) =>
                    `${styles.link} ${isActive ? styles.active : ''}`
                  }
                  to="/login"
                >
                  Login
                </NavLink>
              </li>
            )}
            {isAdmin && (
              <li>
                <NavLink
                  className={({ isActive }) =>
                    `${styles.link} ${isActive ? styles.active : ''}`
                  }
                  to="/admin"
                >
                  Painel Admin
                </NavLink>
              </li>
            )}
            {isAuthenticated && (
              <li>
                <button className={`${styles.link} ${styles.logout}`} onClick={handleLogout} type="button">
                  Logout
                </button>
              </li>
            )}
          </ul>
        </nav>
      </div>
    </header>
  )
}

export default Header
