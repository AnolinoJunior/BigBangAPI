import { Link } from 'react-router-dom'
import styles from './HomePage.module.css'

const resources = [
  {
    title: 'Characters',
    description: 'Conheça os personagens e os detalhes de cada um.',
    to: '/characters',
  },
  {
    title: 'Episodes',
    description: 'Explore os episódios por temporada e número.',
    to: '/episodes',
  },
  {
    title: 'Locations',
    description: 'Veja os locais e os personagens que vivem neles.',
    to: '/locations',
  },
]

function HomePage() {
  return (
    <section className={styles.home} aria-labelledby="home-title">
      <div className={styles.hero}>
        <p className={styles.eyebrow}>The Big Bang Theory</p>

        <h1 id="home-title">
          Tudo sobre The Big Bang Theory
        </h1>

        <p className={styles.introduction}>
          Um catálogo simples para navegar por personagens, episódios e locais.
        </p>
      </div>

      <div className={styles.cards}>
        {resources.map((resource) => (
          <Link
            className={styles.card}
            key={resource.to}
            to={resource.to}
          >
            <h2>{resource.title}</h2>
            <p>{resource.description}</p>

            <span
              className={styles.explore}
              aria-hidden="true"
            >
              Explorar →
            </span>
          </Link>
        ))}
      </div>
    </section>
  )
}

export default HomePage