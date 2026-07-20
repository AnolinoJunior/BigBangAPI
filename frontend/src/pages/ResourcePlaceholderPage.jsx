import { Link } from 'react-router-dom'
import styles from './ResourcePlaceholderPage.module.css'

function ResourcePlaceholderPage({ resourceName }) {
  return (
    <section className={styles.page} aria-labelledby="resource-title">
      <p className={styles.eyebrow}>Em breve</p>
      <h1 id="resource-title">{resourceName}</h1>
      <p>
        Esta se\u00e7\u00e3o receber\u00e1 sua listagem e opera\u00e7\u00f5es nas pr\u00f3ximas etapas.
      </p>
      <Link className={styles.backLink} to="/">
        Voltar para a Home
      </Link>
    </section>
  )
}

export default ResourcePlaceholderPage
