import { useEffect, useState } from 'react'
import CharacterCard from '../components/characters/CharacterCard'
import Pagination from '../components/common/Pagination'
import { getCharacters } from '../services/characterService'
import styles from './CharactersPage.module.css'

const PAGE_SIZE = 6;

function CharactersPage() {
  const [characters, setCharacters] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
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
        })

        if (!ignoreResponse) {
          setCharacters(charactersPage)
        }
      } catch (error) {
        if (ignoreResponse) {
          return
        }

        if (error.response?.status === 404) {
          setCharacters([])
          return
        }

        setCharacters([])
        setErrorMessage('Não foi possível carregar os personagens. Tente novamente.')
      } finally {
        if (!ignoreResponse) {
          setIsLoading(false)
        }
      }
    }

    loadCharacters()

    return () => {
      ignoreResponse = true
    }
  }, [currentPage, reloadKey])

  const hasNextPage = characters.length === PAGE_SIZE
  const isEmpty = !isLoading && !errorMessage && characters.length === 0

  return (
    <section className={styles.page} aria-labelledby="characters-title">
      <header className={styles.header}>
        <p className={styles.eyebrow}>Catálogo</p>
        <h1 id="characters-title">Characters</h1>
        <p>Conheça os personagens cadastrados na Big Bang API.</p>
      </header>

      {isLoading && <p className={styles.status}>Carregando personagens...</p>}

      {errorMessage && (
        <div className={`${styles.status} ${styles.error}`} role="alert">
          <p>{errorMessage}</p>
          <button onClick={() => setReloadKey((key) => key + 1)} type="button">
            Tentar novamente
          </button>
        </div>
      )}

      {isEmpty && (
        <p className={styles.status}>
          Nenhum personagem foi encontrado na página {currentPage}.
        </p>
      )}

      {!isLoading && !errorMessage && characters.length > 0 && (
        <>
          <div className={styles.grid}>
            {characters.map((character) => (
              <CharacterCard character={character} key={character.id} />
            ))}
          </div>
          <Pagination
            currentPage={currentPage}
            hasNextPage={hasNextPage}
            isLoading={isLoading}
            onPageChange={setCurrentPage}
          />
        </>
      )}

      {isEmpty && currentPage > 1 && (
        <Pagination
          currentPage={currentPage}
          hasNextPage={false}
          isLoading={isLoading}
          onPageChange={setCurrentPage}
        />
      )}
    </section>
  )
}

export default CharactersPage
