import { Link } from 'react-router-dom'
import styles from './ResourcePlaceholderPage.module.css'

function ResourcePlaceholderPage({ resourceName }) {
  return (
    <section className={styles.page} aria-labelledby="resource-title">
      <p className={styles.eyebrow}>Em breve</p>
      <h1 id="resource-title">{resourceName}</h1>
      <p>
       <p>
       Esta seção receberá sua listagem e operações nas próximas etapas.
      </p>
      </p>
      <Link className={styles.backLink} to="/">
        Voltar para a Home
      </Link>
    </section>
  )
}

export default ResourcePlaceholderPage
