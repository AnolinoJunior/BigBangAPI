import { Link } from 'react-router-dom'
import styles from './HomePage.module.css'

const resources = [
  {
    title: 'Characters',
    description: 'Conhe\u00e7a os personagens e os detalhes de cada um.',
    to: '/characters',
  },
  {
    title: 'Episodes',
    description: 'Explore os epis\u00f3dios por temporada e n\u00famero.',
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
        <h1 id="home-title">Explore o universo da Big Bang API</h1>
        <p className={styles.introduction}>
          Um cat\u00e1logo simples para navegar por personagens, epis\u00f3dios e locais.
        </p>
      </div>

      <div className={styles.cards}>
        {resources.map((resource) => (
          <Link className={styles.card} key={resource.to} to={resource.to}>
            <h2>{resource.title}</h2>
            <p>{resource.description}</p>
            <span aria-hidden="true">Explorar \u2192</span>
          </Link>
        ))}
      </div>
    </section>
  )
}

export default HomePage
