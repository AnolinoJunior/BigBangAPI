import { useEffect, useState } from 'react'
import ConfirmModal from '../components/common/ConfirmModal'
import LocationFormModal from '../components/locations/LocationFormModal'
import {
  createLocation,
  deleteLocation,
  getLocationById,
  getLocations,
  updateLocation,
} from '../services/adminLocationService'

function getApiMessage(error, fallbackMessage) {
  return error.response?.data?.message ?? error.response?.data?.Message ?? fallbackMessage
}

function AdminLocationsPage() {
  const [locations, setLocations] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [errorMessage, setErrorMessage] = useState('')
  const [successMessage, setSuccessMessage] = useState('')
  const [reloadKey, setReloadKey] = useState(0)
  const [formLocation, setFormLocation] = useState(null)
  const [isFormOpen, setIsFormOpen] = useState(false)
  const [isLoadingForm, setIsLoadingForm] = useState(false)
  const [isSaving, setIsSaving] = useState(false)
  const [formError, setFormError] = useState('')
  const [locationToDelete, setLocationToDelete] = useState(null)
  const [isDeleting, setIsDeleting] = useState(false)

  useEffect(() => {
    let ignoreResponse = false

    async function loadLocations() {
      setIsLoading(true)
      setErrorMessage('')

      try {
        const allLocations = await getLocations()

        if (!ignoreResponse) {
          setLocations(allLocations)
        }
      } catch (error) {
        if (!ignoreResponse) {
          setErrorMessage(getApiMessage(error, 'Não foi possível carregar as locations.'))
        }
      } finally {
        if (!ignoreResponse) setIsLoading(false)
      }
    }

    loadLocations()

    return () => { ignoreResponse = true }
  }, [reloadKey])

  async function openEditForm(locationId) {
    setErrorMessage('')
    setFormError('')
    setIsLoadingForm(true)

    try {
      const location = await getLocationById(locationId)
      setFormLocation(location)
      setIsFormOpen(true)
    } catch (error) {
      setErrorMessage(getApiMessage(error, 'Não foi possível carregar a location para edição.'))
    } finally {
      setIsLoadingForm(false)
    }
  }

  function openCreateForm() {
    setErrorMessage('')
    setFormError('')
    setFormLocation(null)
    setIsFormOpen(true)
  }

  async function handleSave(locationData) {
    setIsSaving(true)
    setErrorMessage('')
    setFormError('')

    try {
      if (formLocation) {
        await updateLocation(formLocation.id, locationData)
        setSuccessMessage('Location atualizada com sucesso.')
      } else {
        await createLocation(locationData)
        setSuccessMessage('Location criada com sucesso.')
      }

      setIsFormOpen(false)
      setFormLocation(null)
      setReloadKey((key) => key + 1)
    } catch (error) {
      setFormError(getApiMessage(error, 'Não foi possível salvar a location.'))
    } finally {
      setIsSaving(false)
    }
  }

  async function handleDelete() {
    if (!locationToDelete) return

    setIsDeleting(true)
    setErrorMessage('')

    try {
      await deleteLocation(locationToDelete.id)
      setSuccessMessage('Location excluída com sucesso.')
      setLocationToDelete(null)
      setReloadKey((key) => key + 1)
    } catch (error) {
      setErrorMessage(getApiMessage(error, 'Não foi possível excluir a location.'))
      setLocationToDelete(null)
    } finally {
      setIsDeleting(false)
    }
  }

  return (
    <section>
      <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4">
        <div>
          <h1 className="h3 mb-1">Locations Administration</h1>
          <p className="text-body-secondary mb-0">Gerencie as locations cadastradas na API.</p>
        </div>
        <button className="btn btn-primary" disabled={isLoadingForm} onClick={openCreateForm} type="button">
          {isLoadingForm ? 'Carregando...' : 'Nova Location'}
        </button>
      </div>

      {successMessage && <div className="alert alert-success" role="status">{successMessage}</div>}
      {errorMessage && <div className="alert alert-danger" role="alert">{errorMessage}</div>}

      {isLoading && <div className="text-center py-5"><div aria-label="Carregando locations" className="spinner-border" role="status" /></div>}

      {!isLoading && !errorMessage && locations.length === 0 && (
        <div className="alert alert-info mb-0">Nenhuma location foi encontrada.</div>
      )}

      {!isLoading && locations.length > 0 && (
        <div className="card shadow-sm">
          <div className="table-responsive">
            <table className="table table-hover align-middle mb-0">
              <thead>
                <tr>
                  <th scope="col">Nome</th>
                  <th className="text-end" scope="col">Ações</th>
                </tr>
              </thead>
              <tbody>
                {locations.map((location) => (
                  <tr key={location.id}>
                    <td className="fw-semibold">{location.name}</td>
                    <td className="text-end text-nowrap">
                      <button className="btn btn-sm btn-outline-primary me-2" disabled={isLoadingForm} onClick={() => openEditForm(location.id)} type="button">Editar</button>
                      <button className="btn btn-sm btn-outline-danger" onClick={() => setLocationToDelete(location)} type="button">Excluir</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {isFormOpen && <LocationFormModal errorMessage={formError} isSaving={isSaving} location={formLocation} onCancel={() => setIsFormOpen(false)} onSave={handleSave} />}
      {locationToDelete && <ConfirmModal isSubmitting={isDeleting} message={`Deseja excluir ${locationToDelete.name}? Esta ação não pode ser desfeita.`} onCancel={() => setLocationToDelete(null)} onConfirm={handleDelete} title="Excluir Location" />}
    </section>
  )
}

export default AdminLocationsPage
