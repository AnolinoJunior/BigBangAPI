import { Route, Routes } from 'react-router-dom'
import AppLayout from '../components/layout/AppLayout'
import CharactersPage from '../pages/CharactersPage'
import HomePage from '../pages/HomePage'
import ResourcePlaceholderPage from '../pages/ResourcePlaceholderPage'

function AppRoutes() {
  return (
    <Routes>
      <Route element={<AppLayout />}>
        <Route index element={<HomePage />} />
        <Route path="characters" element={<CharactersPage />} />
        <Route
          path="characters/:characterId"
          element={<ResourcePlaceholderPage resourceName="Detalhes do personagem" />}
        />
        <Route
          path="episodes"
          element={<ResourcePlaceholderPage resourceName="Episodes" />}
        />
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
