import { useEffect, useState } from 'react'
import ConfirmModal from '../components/common/ConfirmModal'
import EpisodeFormModal from '../components/episodes/EpisodeFormModal'
import { getAllEpisodes } from '../services/episodeService'
import { createEpisode, deleteEpisode, updateEpisode } from '../services/adminEpisodeService'

function getApiMessage(error, fallbackMessage) {
  return error.response?.data?.message ?? error.response?.data?.Message ?? fallbackMessage
}

function AdminEpisodesPage() {
  const [episodes, setEpisodes] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [errorMessage, setErrorMessage] = useState('')
  const [successMessage, setSuccessMessage] = useState('')
  const [reloadKey, setReloadKey] = useState(0)
  const [formEpisode, setFormEpisode] = useState(null)
  const [isFormOpen, setIsFormOpen] = useState(false)
  const [isSaving, setIsSaving] = useState(false)
  const [formError, setFormError] = useState('')
  const [episodeToDelete, setEpisodeToDelete] = useState(null)
  const [isDeleting, setIsDeleting] = useState(false)

  useEffect(() => {
    let ignoreResponse = false

    async function loadEpisodes() {
      setIsLoading(true)
      setErrorMessage('')

      try {
        const allEpisodes = await getAllEpisodes()

        if (!ignoreResponse) {
          setEpisodes(allEpisodes)
        }
      } catch (error) {
        if (!ignoreResponse) {
          setErrorMessage(getApiMessage(error, 'Não foi possível carregar os episódios.'))
        }
      } finally {
        if (!ignoreResponse) setIsLoading(false)
      }
    }

    loadEpisodes()

    return () => { ignoreResponse = true }
  }, [reloadKey])

  function openForm(episode = null) {
    setErrorMessage('')
    setFormError('')
    setFormEpisode(episode)
    setIsFormOpen(true)
  }

  async function handleSave(episodeData) {
    setIsSaving(true)
    setErrorMessage('')
    setFormError('')

    try {
      if (formEpisode) {
        await updateEpisode(formEpisode.id, episodeData)
        setSuccessMessage('Episódio atualizado com sucesso.')
      } else {
        await createEpisode(episodeData)
        setSuccessMessage('Episódio criado com sucesso.')
      }

      setIsFormOpen(false)
      setFormEpisode(null)
      setReloadKey((key) => key + 1)
    } catch (error) {
      setFormError(getApiMessage(error, 'Não foi possível salvar o episódio.'))
    } finally {
      setIsSaving(false)
    }
  }

  async function handleDelete() {
    if (!episodeToDelete) return

    setIsDeleting(true)
    setErrorMessage('')

    try {
      await deleteEpisode(episodeToDelete.id)
      setSuccessMessage('Episódio excluído com sucesso.')
      setEpisodeToDelete(null)
      setReloadKey((key) => key + 1)
    } catch (error) {
      setErrorMessage(getApiMessage(error, 'Não foi possível excluir o episódio.'))
      setEpisodeToDelete(null)
    } finally {
      setIsDeleting(false)
    }
  }

  return (
    <section>
      <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4">
        <div>
          <h1 className="h3 mb-1">Administração de episódios</h1>
          <p className="text-body-secondary mb-0">Gerencie os episódios cadastrados na API.</p>
        </div>
        <button className="btn btn-primary" onClick={() => openForm()} type="button">Novo episódio</button>
      </div>

      {successMessage && <div className="alert alert-success" role="status">{successMessage}</div>}
      {errorMessage && <div className="alert alert-danger" role="alert">{errorMessage}</div>}

      {isLoading && <div className="text-center py-5"><div aria-label="Carregando episódios" className="spinner-border" role="status" /></div>}

      {!isLoading && !errorMessage && episodes.length === 0 && (
        <div className="alert alert-info mb-0">Nenhum episódio foi encontrado.</div>
      )}

      {!isLoading && episodes.length > 0 && (
        <div className="card shadow-sm">
          <div className="table-responsive">
            <table className="table table-hover align-middle mb-0">
              <thead>
                <tr>
                  <th scope="col">Título</th>
                  <th scope="col">Temporada</th>
                  <th scope="col">Episódio</th>
                  <th className="text-end" scope="col">Ações</th>
                </tr>
              </thead>
              <tbody>
                {episodes.map((episode) => (
                  <tr key={episode.id}>
                    <td className="fw-semibold">{episode.title}</td>
                    <td>{episode.season}</td>
                    <td>{episode.episodeNumber}</td>
                    <td className="text-end text-nowrap">
                      <button className="btn btn-sm btn-outline-primary me-2" onClick={() => openForm(episode)} type="button">Editar</button>
                      <button className="btn btn-sm btn-outline-danger" onClick={() => setEpisodeToDelete(episode)} type="button">Excluir</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {isFormOpen && <EpisodeFormModal episode={formEpisode} errorMessage={formError} isSaving={isSaving} onCancel={() => setIsFormOpen(false)} onSave={handleSave} />}
      {episodeToDelete && <ConfirmModal isSubmitting={isDeleting} message={`Deseja excluir ${episodeToDelete.title}? Esta ação não pode ser desfeita.`} onCancel={() => setEpisodeToDelete(null)} onConfirm={handleDelete} title="Excluir episódio" />}
    </section>
  )
}

export default AdminEpisodesPage
