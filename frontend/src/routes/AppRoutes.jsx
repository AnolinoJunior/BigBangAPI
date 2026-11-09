import { Route, Routes } from 'react-router-dom'
import AdminLayout from '../components/layout/AdminLayout'
import AdminDashboardPage from '../pages/AdminDashboardPage'
import AdminCharactersPage from '../pages/AdminCharactersPage'
import AdminEpisodesPage from '../pages/AdminEpisodesPage'
import AdminLocationsPage from '../pages/AdminLocationsPage'
import AppLayout from '../components/layout/AppLayout'
import CharacterDetailsPage from '../pages/CharacterDetailsPage'
import CharactersPage from '../pages/CharactersPage'
import HomePage from '../pages/HomePage'
import LoginPage from '../pages/LoginPage'
import ResourcePlaceholderPage from '../pages/ResourcePlaceholderPage'
import SeasonEpisodesPage from '../pages/SeasonEpisodesPage'
import SeasonsPage from '../pages/SeasonsPage'
import AdminRoute from './AdminRoute'
import ProtectedRoute from './ProtectedRoute'

function AppRoutes() {
  return (
    <Routes>
      <Route path="login" element={<LoginPage />} />
      <Route element={<ProtectedRoute />}>
        <Route element={<AdminRoute />}>
          <Route path="admin" element={<AdminLayout />}>
            <Route index element={<AdminDashboardPage />} />
            <Route path="characters" element={<AdminCharactersPage />} />
            <Route path="episodes" element={<AdminEpisodesPage />} />
            <Route path="locations" element={<AdminLocationsPage />} />
          </Route>
        </Route>
      </Route>
      <Route element={<AppLayout />}>
        <Route index element={<HomePage />} />
        <Route path="characters" element={<CharactersPage />} />
        <Route path="characters/:characterId" element={<CharacterDetailsPage />} />
        <Route path="episodes" element={<SeasonsPage />} />
        <Route path="episodes/season/:seasonNumber" element={<SeasonEpisodesPage />} />
        <Route
          path="locations"
          element={<ResourcePlaceholderPage resourceName="Locations" />}
        />
        <Route path="*" element={<HomePage />} />
      </Route>
    </Routes>
  )
}

export default AppRoutes
