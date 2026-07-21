import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import { getAllEpisodes } from '../services/episodeService'
import styles from './SeasonEpisodesPage.module.css'

function SeasonEpisodesPage() {
  const { seasonNumber } = useParams()
  const [episodes, setEpisodes] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [errorMessage, setErrorMessage] = useState('')
  const [reloadKey, setReloadKey] = useState(0)

  useEffect(() => {
    let ignoreResponse = false

    async function loadSeasonEpisodes() {
      setIsLoading(true)
      setErrorMessage('')

      try {
        const allEpisodes = await getAllEpisodes()
        const episodesInSeason = allEpisodes
          .filter((episode) => episode.season === Number(seasonNumber))
          .sort((firstEpisode, secondEpisode) => firstEpisode.episodeNumber - secondEpisode.episodeNumber)

        if (!ignoreResponse) {
          setEpisodes(episodesInSeason)
        }
      } catch {
        if (!ignoreResponse) {
          setEpisodes([])
          setErrorMessage('Não foi possível carregar os episódios da temporada. Tente novamente.')
        }
      } finally {
        if (!ignoreResponse) {
          setIsLoading(false)
        }
      }
    }

    loadSeasonEpisodes()

    return () => {
      ignoreResponse = true
    }
  }, [reloadKey, seasonNumber])

  return (
    <section className={styles.page} aria-labelledby="season-title">
      <Link className={styles.backLink} to="/episodes">
        ← Voltar para temporadas
      </Link>

      <header className={styles.header}>
        <p className={styles.eyebrow}>Temporada</p>
        <h1 id="season-title">Temporada {seasonNumber}</h1>
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

      {!isLoading && !errorMessage && episodes.length === 0 && (
        <p className={styles.status}>Nenhum episódio foi encontrado para esta temporada.</p>
      )}

      {!isLoading && episodes.length > 0 && (
        <ol className={styles.list}>
          {episodes.map((episode) => (
            <li key={episode.id}>
              <p>Episódio {episode.episodeNumber}</p>
              <h2>{episode.title}</h2>
            </li>
          ))}
        </ol>
      )}
    </section>
  )
}

export default SeasonEpisodesPage
