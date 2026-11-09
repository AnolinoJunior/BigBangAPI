import { useEffect, useState } from 'react'
import CharacterFormModal from '../components/characters/CharacterFormModal'
import ConfirmModal from '../components/common/ConfirmModal'
import Pagination from '../components/common/Pagination'
import { getCharacterImageUrl, getCharacterById, getCharacters } from '../services/characterService'
import {
  createCharacter,
  deleteCharacter,
  getLocationsForCharacterForm,
  updateCharacter,
} from '../services/adminCharacterService'
import { uploadCharacterImage } from '../services/uploadService'

const PAGE_SIZE = 6

function getApiMessage(error, fallbackMessage) {
  return error.response?.data?.message ?? error.response?.data?.Message ?? fallbackMessage
}

function AdminCharactersPage() {
  const [characters, setCharacters] = useState([])
  const [pagination, setPagination] = useState(null)
  const [currentPage, setCurrentPage] = useState(1)
  const [isLoading, setIsLoading] = useState(true)
  const [errorMessage, setErrorMessage] = useState('')
  const [successMessage, setSuccessMessage] = useState('')
  const [reloadKey, setReloadKey] = useState(0)
  const [formCharacter, setFormCharacter] = useState(null)
  const [isFormOpen, setIsFormOpen] = useState(false)
  const [locations, setLocations] = useState([])
  const [isLoadingForm, setIsLoadingForm] = useState(false)
  const [isSaving, setIsSaving] = useState(false)
  const [isUploading, setIsUploading] = useState(false)
  const [formError, setFormError] = useState('')
  const [characterToDelete, setCharacterToDelete] = useState(null)
  const [isDeleting, setIsDeleting] = useState(false)

  useEffect(() => {
    let ignoreResponse = false

    async function loadCharacters() {
      setIsLoading(true)
      setErrorMessage('')

      try {
        const charactersPage = await getCharacters({ pageNumber: currentPage, pageSize: PAGE_SIZE })
        const charactersWithDetails = await Promise.all(
          charactersPage.characters.map((character) => getCharacterById(character.id)),
        )

        if (!ignoreResponse) {
          setCharacters(charactersWithDetails)
          setPagination(charactersPage.pagination)
        }
      } catch (error) {
        if (ignoreResponse) return

        if (error.response?.status === 404) {
          setCharacters([])
          setPagination(null)
        } else {
          setErrorMessage(getApiMessage(error, 'Não foi possível carregar os characters.'))
        }
      } finally {
        if (!ignoreResponse) setIsLoading(false)
      }
    }

    loadCharacters()

    return () => { ignoreResponse = true }
  }, [currentPage, reloadKey])

  async function openForm(character = null) {
    setErrorMessage('')
    setFormError('')
    setIsLoadingForm(true)

    try {
      const availableLocations = await getLocationsForCharacterForm()
      setLocations(availableLocations)
      setFormCharacter(character)
      setIsFormOpen(true)
    } catch (error) {
      setErrorMessage(getApiMessage(error, 'Não foi possível carregar as localizações para o formulário.'))
    } finally {
      setIsLoadingForm(false)
    }
  }

  async function handleSave({ characterData, imageFile }) {
    setErrorMessage('')
    setFormError('')

    try {
      let characterToSave = characterData

      if (imageFile) {
        setIsUploading(true)
        const imageUrl = await uploadCharacterImage(imageFile)
        characterToSave = { ...characterData, imageUrl }
        setIsUploading(false)
      }

      setIsSaving(true)

      if (formCharacter) {
        await updateCharacter(formCharacter.id, characterToSave)
        setSuccessMessage('Character atualizado com sucesso.')
      } else {
        await createCharacter(characterToSave)
        setSuccessMessage('Character criado com sucesso.')
      }

      setIsFormOpen(false)
      setFormCharacter(null)
      setReloadKey((key) => key + 1)
    } catch (error) {
      setFormError(getApiMessage(error, 'Não foi possível salvar o character.'))
    } finally {
      setIsUploading(false)
      setIsSaving(false)
    }
  }

  async function handleDelete() {
    if (!characterToDelete) return

    setIsDeleting(true)
    setErrorMessage('')

    try {
      await deleteCharacter(characterToDelete.id)
      setSuccessMessage('Character excluído com sucesso.')
      setCharacterToDelete(null)

      if (characters.length === 1 && currentPage > 1) {
        setCurrentPage((page) => page - 1)
      } else {
        setReloadKey((key) => key + 1)
      }
    } catch (error) {
      setErrorMessage(getApiMessage(error, 'Não foi possível excluir o character.'))
      setCharacterToDelete(null)
    } finally {
      setIsDeleting(false)
    }
  }

  return (
    <section>
      <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4">
        <div>
          <h1 className="h3 mb-1">Characters Administration</h1>
          <p className="text-body-secondary mb-0">Gerencie os characters cadastrados na API.</p>
        </div>
        <button className="btn btn-primary" disabled={isLoadingForm} onClick={() => openForm()} type="button">
          {isLoadingForm ? 'Carregando...' : 'Novo Character'}
        </button>
      </div>

      {successMessage && <div className="alert alert-success" role="status">{successMessage}</div>}
      {errorMessage && <div className="alert alert-danger" role="alert">{errorMessage}</div>}

      {isLoading && <div className="text-center py-5"><div aria-label="Carregando characters" className="spinner-border" role="status" /></div>}

      {!isLoading && !errorMessage && characters.length === 0 && (
        <div className="alert alert-info mb-0">Nenhum character foi encontrado.</div>
      )}

      {!isLoading && characters.length > 0 && (
        <div className="card shadow-sm">
          <div className="table-responsive">
            <table className="table table-hover align-middle mb-0">
              <thead>
                <tr>
                  <th scope="col">Imagem</th>
                  <th scope="col">Nome</th>
                  <th scope="col">Ator</th>
                  <th scope="col">Ocupação</th>
                  <th scope="col">Localização</th>
                  <th className="text-end" scope="col">Ações</th>
                </tr>
              </thead>
              <tbody>
                {characters.map((character) => (
                  <tr key={character.id}>
                    <td>
                      {character.imageUrl ? <img alt="" className="rounded object-fit-cover" height="48" src={getCharacterImageUrl(character.imageUrl)} width="48" /> : <span className="text-body-secondary">—</span>}
                    </td>
                    <td className="fw-semibold">{character.name}</td>
                    <td>{character.actor}</td>
                    <td>{character.occupation}</td>
                    <td>{character.locationName ?? 'Não informado'}</td>
                    <td className="text-end text-nowrap">
                      <button className="btn btn-sm btn-outline-primary me-2" onClick={() => openForm(character)} type="button">Editar</button>
                      <button className="btn btn-sm btn-outline-danger" onClick={() => setCharacterToDelete(character)} type="button">Excluir</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {pagination && (
        <Pagination
          currentPage={pagination.CurrentPage}
          hasNextPage={pagination.HasNext}
          hasPreviousPage={pagination.HasPrevious}
          isLoading={isLoading}
          onPageChange={setCurrentPage}
          totalPages={pagination.TotalPages}
        />
      )}

      {isFormOpen && <CharacterFormModal character={formCharacter} errorMessage={formError} isSaving={isSaving} isUploading={isUploading} locations={locations} onCancel={() => setIsFormOpen(false)} onSave={handleSave} />}
      {characterToDelete && <ConfirmModal isSubmitting={isDeleting} message={`Deseja excluir ${characterToDelete.name}? Esta ação não pode ser desfeita.`} onCancel={() => setCharacterToDelete(null)} onConfirm={handleDelete} title="Excluir Character" />}
    </section>
  )
}

export default AdminCharactersPage
