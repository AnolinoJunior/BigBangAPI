import styles from './EpisodeCard.module.css'

function EpisodeCard({ episode }) {
  return (
    <article className={styles.card}>
      <p className={styles.identifier}>
        Temporada {episode.season} · Episódio {episode.episodeNumber}
      </p>
      <h2>{episode.title}</h2>
      <dl className={styles.details}>
        <div>
          <dt>Temporada</dt>
          <dd>{episode.season}</dd>
        </div>
        <div>
          <dt>Número do episódio</dt>
          <dd>{episode.episodeNumber}</dd>
        </div>
      </dl>
    </article>
  )
}

export default EpisodeCard
