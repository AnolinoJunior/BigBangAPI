import { Link } from 'react-router-dom'
import placeholderImage from '../../assets/character-placeholder.svg'
import { getCharacterImageUrl } from '../../services/characterService'
import styles from './CharacterCard.module.css'

function CharacterCard({ character }) {
  const imageSource = getCharacterImageUrl(character.imageUrl) ?? placeholderImage

  function showPlaceholderOnError(event) {
    event.currentTarget.onerror = null
    event.currentTarget.src = placeholderImage
  }

  return (
    <article className={styles.card}>
      <img
        className={styles.image}
        src={imageSource}
        alt={`Retrato de ${character.name}`}
        onError={showPlaceholderOnError}
      />
      <div className={styles.content}>
        <h2>{character.name}</h2>
        <dl className={styles.details}>
          <div>
            <dt>Ator</dt>
            <dd>{character.actor}</dd>
          </div>
          <div>
            <dt>Ocupação</dt>
            <dd>{character.occupation}</dd>
          </div>
        </dl>
        <Link className={styles.detailsLink} to={`/characters/${character.id}`}>
          Ver detalhes
        </Link>
      </div>
    </article>
  )
}

export default CharacterCard
