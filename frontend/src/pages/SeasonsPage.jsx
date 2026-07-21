import { useEffect, useState } from 'react'
import SeasonCard from '../components/episodes/SeasonCard'
import { getAllEpisodes } from '../services/episodeService'
import styles from './SeasonsPage.module.css'

function SeasonsPage() {
  const [seasons, setSeasons] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [errorMessage, setErrorMessage] = useState('')
  const [reloadKey, setReloadKey] = useState(0)

  useEffect(() => {
    let ignoreResponse = false

    async function loadSeasons() {
      setIsLoading(true)
      setErrorMessage('')

      try {
        const episodes = await getAllEpisodes()
        const groupedSeasons = episodes.reduce((groups, episode) => {
          groups.set(episode.season, (groups.get(episode.season) ?? 0) + 1)
          return groups
        }, new Map())

        if (!ignoreResponse) {
          setSeasons(
            [...groupedSeasons.entries()]
              .map(([seasonNumber, episodeCount]) => ({ seasonNumber, episodeCount }))
              .sort((firstSeason, secondSeason) => firstSeason.seasonNumber - secondSeason.seasonNumber),
          )
        }
      } catch {
        if (!ignoreResponse) {
          setSeasons([])
          setErrorMessage('Não foi possível carregar as temporadas. Tente novamente.')
        }
      } finally {
        if (!ignoreResponse) {
          setIsLoading(false)
        }
      }
    }

    loadSeasons()

    return () => {
      ignoreResponse = true
    }
  }, [reloadKey])

  return (
    <section className={styles.page} aria-labelledby="seasons-title">
      <header className={styles.header}>
        <p className={styles.eyebrow}>Catálogo</p>
        <h1 id="seasons-title">Temporadas</h1>
        <p>Selecione uma temporada para explorar seus episódios.</p>
      </header>

      {isLoading && <p className={styles.status}>Carregando temporadas...</p>}

      {errorMessage && (
        <div className={`${styles.status} ${styles.error}`} role="alert">
          <p>{errorMessage}</p>
          <button onClick={() => setReloadKey((key) => key + 1)} type="button">
            Tentar novamente
          </button>
        </div>
      )}

      {!isLoading && !errorMessage && seasons.length === 0 && (
        <p className={styles.status}>Nenhuma temporada foi encontrada.</p>
      )}

      {!isLoading && seasons.length > 0 && (
        <div className={styles.grid}>
          {seasons.map((season) => (
            <SeasonCard
              episodeCount={season.episodeCount}
              key={season.seasonNumber}
              seasonNumber={season.seasonNumber}
            />
          ))}
        </div>
      )}
    </section>
  )
}

export default SeasonsPage
