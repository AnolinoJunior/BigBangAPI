import { useEffect, useState } from 'react'
import EpisodeCard from '../components/episodes/EpisodeCard'
import Pagination from '../components/common/Pagination'
import { getEpisodes } from '../services/episodeService'
import styles from './EpisodesPage.module.css'

const PAGE_SIZE = 10

function EpisodesPage() {
  const [episodes, setEpisodes] = useState([])
  const [currentPage, setCurrentPage] = useState(1)
  const [isLoading, setIsLoading] = useState(true)
  const [errorMessage, setErrorMessage] = useState('')
  const [reloadKey, setReloadKey] = useState(0)

  useEffect(() => {
    let ignoreResponse = false

    async function loadEpisodes() {
      setIsLoading(true)
      setErrorMessage('')

      try {
        const episodesPage = await getEpisodes({
          pageNumber: currentPage,
          pageSize: PAGE_SIZE,
        })

        if (!ignoreResponse) {
          setEpisodes(episodesPage)
        }
      } catch (error) {
        if (ignoreResponse) {
          return
        }

        if (error.response?.status === 404) {
          setEpisodes([])
          return
        }

        setEpisodes([])
        setErrorMessage('Não foi possível carregar os episódios. Tente novamente.')
      } finally {
        if (!ignoreResponse) {
          setIsLoading(false)
        }
      }
    }

    loadEpisodes()

    return () => {
      ignoreResponse = true
    }
  }, [currentPage, reloadKey])

  const hasNextPage = episodes.length === PAGE_SIZE
  const isEmpty = !isLoading && !errorMessage && episodes.length === 0

  return (
    <section className={styles.page} aria-labelledby="episodes-title">
      <header className={styles.header}>
        <p className={styles.eyebrow}>Catálogo</p>
        <h1 id="episodes-title">Episodes</h1>
        <p>Explore os episódios cadastrados na Big Bang API.</p>
      </header>

      {isLoading && <p className={styles.status}>Carregando episódios...</p>}

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
          Nenhum episódio foi encontrado na página {currentPage}.
        </p>
      )}

      {!isLoading && !errorMessage && episodes.length > 0 && (
        <>
          <div className={styles.grid}>
            {episodes.map((episode) => (
              <EpisodeCard episode={episode} key={episode.id} />
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

export default EpisodesPage
