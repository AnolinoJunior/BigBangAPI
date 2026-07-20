import { NavLink } from 'react-router-dom'
import styles from './Header.module.css'

const navigationItems = [
  { label: 'Home', to: '/' },
  { label: 'Characters', to: '/characters' },
  { label: 'Episodes', to: '/episodes' },
  { label: 'Locations', to: '/locations' },
]

function Header() {
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
          </ul>
        </nav>
      </div>
    </header>
  )
}

export default Header
