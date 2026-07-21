import { Link } from 'react-router-dom'
import styles from './SeasonCard.module.css'

function SeasonCard({ seasonNumber, episodeCount }) {
  return (
    <Link className={styles.card} to={`/episodes/season/${seasonNumber}`}>
      <span className={styles.icon} aria-hidden="true">
        📺
      </span>
      <div>
        <h2>Temporada {seasonNumber}</h2>
        <p>
          {episodeCount} {episodeCount === 1 ? 'episódio' : 'episódios'}
        </p>
      </div>
      <span className={styles.action} aria-hidden="true">
        Ver episódios →
      </span>
    </Link>
  )
}

export default SeasonCard
