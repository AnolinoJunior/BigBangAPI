import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import placeholderImage from '../assets/character-placeholder.svg'
import { getCharacterById, getCharacterImageUrl } from '../services/characterService'
import { getAllEpisodes } from '../services/episodeService'
import styles from './CharacterDetailsPage.module.css'

function groupEpisodesBySeason(characterEpisodes, allEpisodes) {
  const characterEpisodeTitles = new Set(
    characterEpisodes.map((episode) => (typeof episode === 'string' ? episode : episode.title)),
  )
  const episodesBySeason = new Map()

  allEpisodes
    .filter((episode) => characterEpisodeTitles.has(episode.title))
    .forEach((episode) => {
      const seasonEpisodes = episodesBySeason.get(episode.season) ?? []
      seasonEpisodes.push(episode)
      episodesBySeason.set(episode.season, seasonEpisodes)
    })

  return [...episodesBySeason.entries()]
    .map(([season, episodes]) => ({
      season,
      episodes: episodes.sort((firstEpisode, secondEpisode) => firstEpisode.episodeNumber - secondEpisode.episodeNumber),
    }))
    .sort((firstSeason, secondSeason) => firstSeason.season - secondSeason.season)
}

function CharacterDetailsPage() {
  const { characterId } = useParams()
  const [character, setCharacter] = useState(null)
  const [isLoading, setIsLoading] = useState(true)
  const [isNotFound, setIsNotFound] = useState(false)
  const [errorMessage, setErrorMessage] = useState('')
  const [episodesBySeason, setEpisodesBySeason] = useState([])
  const [openSeason, setOpenSeason] = useState(null)

  useEffect(() => {
    let ignoreResponse = false

    async function loadCharacter() {
      setIsLoading(true)
      setIsNotFound(false)
      setErrorMessage('')

      try {
        const [characterDetails, allEpisodes] = await Promise.all([
          getCharacterById(characterId),
          getAllEpisodes(),
        ])

        if (!ignoreResponse) {
          setCharacter(characterDetails)
          setEpisodesBySeason(groupEpisodesBySeason(characterDetails.episodes ?? [], allEpisodes))
          setOpenSeason(null)
        }
      } catch (error) {
        if (ignoreResponse) {
          return
        }

        setCharacter(null)
        setEpisodesBySeason([])

        if (error.response?.status === 404) {
          setIsNotFound(true)
          return
        }

        setErrorMessage('Não foi possível carregar os detalhes do personagem.')
      } finally {
        if (!ignoreResponse) {
          setIsLoading(false)
        }
      }
    }

    loadCharacter()

    return () => {
      ignoreResponse = true
    }
  }, [characterId])

  function showPlaceholderOnError(event) {
    event.currentTarget.onerror = null
    event.currentTarget.src = placeholderImage
  }

  function toggleSeason(season) {
    setOpenSeason((currentSeason) => (currentSeason === season ? null : season))
  }

  if (isLoading) {
    return <p className={styles.status}>Carregando personagem...</p>
  }

  if (isNotFound) {
    return (
      <section className={styles.feedback} aria-labelledby="not-found-title">
        <p className={styles.eyebrow}>404</p>
        <h1 id="not-found-title">Personagem não encontrado</h1>
        <p>O personagem solicitado não está disponível na Big Bang API.</p>
        <Link className={styles.backLink} to="/characters">
          Voltar para Characters
        </Link>
      </section>
    )
  }

  if (errorMessage) {
    return (
      <section className={styles.feedback} aria-labelledby="error-title">
        <p className={styles.eyebrow}>Erro</p>
        <h1 id="error-title">Não foi possível carregar o personagem</h1>
        <p>{errorMessage}</p>
        <Link className={styles.backLink} to="/characters">
          Voltar para Characters
        </Link>
      </section>
    )
  }

  const imageSource = getCharacterImageUrl(character.imageUrl) ?? placeholderImage
  const information = [
    { label: 'Ator', value: character.actor },
    { label: 'Ocupação', value: character.occupation },
    { label: 'Universidade', value: character.university || 'Não informada' },
    { label: 'Idade', value: `${character.age} anos` },
    { label: 'IQ', value: character.iq },
    { label: 'Localização', value: character.locationName || 'Não informada' },
  ]

  return (
    <section className={styles.page} aria-labelledby="character-name">
      <Link className={styles.backLink} to="/characters">
        ← Voltar para Characters
      </Link>

      <div className={styles.profile}>
        <img
          className={styles.image}
          src={imageSource}
          alt={`Retrato de ${character.name}`}
          onError={showPlaceholderOnError}
        />

        <div className={styles.summary}>
          <p className={styles.eyebrow}>Personagem</p>
          <h1 id="character-name">{character.name}</h1>
          <dl className={styles.information}>
            {information.map((item) => (
              <div key={item.label}>
                <dt>{item.label}</dt>
                <dd>{item.value}</dd>
              </div>
            ))}
          </dl>
        </div>
      </div>

      <section className={styles.episodes} aria-labelledby="episodes-title">
        <h2 id="episodes-title">Participações</h2>
        {character.episodes?.length > 0 ? (
          <div className={styles.accordion}>
            {episodesBySeason.map(({ season, episodes }) => {
              const isOpen = openSeason === season

              return (
                <div className={styles.season} key={season}>
                  <button
                    aria-expanded={isOpen}
                    className={styles.seasonButton}
                    onClick={() => toggleSeason(season)}
                    type="button"
                  >
                    <span aria-hidden="true" className={styles.seasonIndicator}>
                      {isOpen ? '▼' : '▶'}
                    </span>
                    Temporada {season}
                  </button>

                  {isOpen && (
                    <ul className={styles.episodeList}>
                      {episodes.map((episode) => (
                        <li key={episode.id}>
                          Episódio {episode.episodeNumber} - {episode.title}
                        </li>
                      ))}
                    </ul>
                  )}
                </div>
              )
            })}
          </div>
        ) : (
          <p>Nenhum episódio informado para este personagem.</p>
        )}
      </section>
    </section>
  )
}

export default CharacterDetailsPage
