import { useEffect, useState } from 'react'
import { Link, useParams } from 'react-router-dom'
import placeholderImage from '../assets/character-placeholder.svg'
import { getCharacterById, getCharacterImageUrl } from '../services/characterService'
import styles from './CharacterDetailsPage.module.css'

function CharacterDetailsPage() {
  const { characterId } = useParams()
  const [character, setCharacter] = useState(null)
  const [isLoading, setIsLoading] = useState(true)
  const [isNotFound, setIsNotFound] = useState(false)
  const [errorMessage, setErrorMessage] = useState('')

  useEffect(() => {
    let ignoreResponse = false

    async function loadCharacter() {
      setIsLoading(true)
      setIsNotFound(false)
      setErrorMessage('')

      try {
        const characterDetails = await getCharacterById(characterId)

        if (!ignoreResponse) {
          setCharacter(characterDetails)
        }
      } catch (error) {
        if (ignoreResponse) {
          return
        }

        setCharacter(null)

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
        <h2 id="episodes-title">Episódios</h2>
        {character.episodes?.length > 0 ? (
          <ul>
            {character.episodes.map((episode) => (
              <li key={episode}>{episode}</li>
            ))}
          </ul>
        ) : (
          <p>Nenhum episódio informado para este personagem.</p>
        )}
      </section>
    </section>
  )
}

export default CharacterDetailsPage
