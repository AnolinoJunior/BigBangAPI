import { useEffect, useState } from 'react'
import CharacterCard from '../components/characters/CharacterCard'
import Pagination from '../components/common/Pagination'
import SearchBar from '../components/common/SearchBar'
import { getCharacters } from '../services/characterService'
import styles from './CharactersPage.module.css'

const PAGE_SIZE = 6

function CharactersPage() {
  const [characters, setCharacters] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
  const [searchTerm, setSearchTerm] = useState('')
  const [pagination, setPagination] = useState(null)
  const [isLoading, setIsLoading] = useState(true)
  const [errorMessage, setErrorMessage] = useState('')
  const [reloadKey, setReloadKey] = useState(0)

  useEffect(() => {
    let ignoreResponse = false

    async function loadCharacters() {
      setIsLoading(true)
      setErrorMessage('')

      try {
        const charactersPage = await getCharacters({
          pageNumber: currentPage,
          pageSize: PAGE_SIZE,
          name: searchTerm,
        })

        if (!ignoreResponse) {
          setCharacters(charactersPage.characters)
          setPagination(charactersPage.pagination)
        }
      } catch (error) {
        if (ignoreResponse) return

        if (error.response?.status === 404) {
          setCharacters([])
          setPagination(null)
          return
        }

        setCharacters([])
        setPagination(null)
        setErrorMessage('Nao foi possivel carregar os personagens. Tente novamente.')
      } finally {
        if (!ignoreResponse) setIsLoading(false)
      }
    }

    loadCharacters()

    return () => { ignoreResponse = true }
  }, [currentPage, reloadKey, searchTerm])

  const isEmpty = !isLoading && !errorMessage && characters.length === 0
  const hasNextPage = pagination?.HasNext ?? false
  const hasPreviousPage = pagination?.HasPrevious ?? false

  function handleSearchChange(value) {
    setSearchTerm(value)
    setCurrentPage(1)
  }

  return (
    <section className={styles.page} aria-labelledby="characters-title">
      <header className={styles.header}>
        <p className={styles.eyebrow}>Catalogo</p>
        <h1 id="characters-title">Characters</h1>
        <p>Conheca os personagens cadastrados na Big Bang API.</p>
      </header>

      <SearchBar onChange={handleSearchChange} onClear={() => handleSearchChange('')} value={searchTerm} />

      {isLoading && <p className={styles.status}>Carregando personagens...</p>}

      {errorMessage && (
        <div className={`${styles.status} ${styles.error}`} role="alert">
          <p>{errorMessage}</p>
          <button onClick={() => setReloadKey((key) => key + 1)} type="button">Tentar novamente</button>
        </div>
      )}

      {isEmpty && <p className={styles.status}>Nenhum personagem foi encontrado{searchTerm ? ` para "${searchTerm}"` : ''}.</p>}

      {!isLoading && !errorMessage && (
        <>
          {characters.length > 0 && (
            <div className={styles.grid}>
              {characters.map((character) => <CharacterCard character={character} key={character.id} />)}
            </div>
          )}
          {pagination && (
            <Pagination
              currentPage={pagination.CurrentPage}
              hasNextPage={hasNextPage}
              hasPreviousPage={hasPreviousPage}
              isLoading={isLoading}
              onPageChange={setCurrentPage}
              totalPages={pagination.TotalPages}
            />
          )}
        </>
      )}
    </section>
  )
}

export default CharactersPage
