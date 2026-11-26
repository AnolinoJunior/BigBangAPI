import { useEffect, useState } from 'react'
import { getCharactersByLocation, getLocations } from '../services/adminLocationService'
import styles from './LocationsPage.module.css'

function LocationsPage() {
  const [locations, setLocations] = useState([])
  const [isLoading, setIsLoading] = useState(true)
  const [errorMessage, setErrorMessage] = useState('')
  const [reloadKey, setReloadKey] = useState(0)

  useEffect(() => {
    let ignoreResponse = false

    async function loadLocations() {
      setIsLoading(true)
      setErrorMessage('')

      try {
        const locationList = await getLocations()
        const locationsWithCharacters = await Promise.all(locationList.map(async (location) => {
          try {
            const characters = await getCharactersByLocation(location.id)
            return { ...location, characters }
          } catch (error) {
            if (error.response?.status === 404) {
              return { ...location, characters: [] }
            }

            throw error
          }
        }))

        if (!ignoreResponse) setLocations(locationsWithCharacters)
      } catch {
        if (!ignoreResponse) {
          setLocations([])
          setErrorMessage('Não foi possível carregar as localizações. Tente novamente.')
        }
      } finally {
        if (!ignoreResponse) setIsLoading(false)
      }
    }

    loadLocations()

    return () => { ignoreResponse = true }
  }, [reloadKey])

  const isEmpty = !isLoading && !errorMessage && locations.length === 0

  return (
    <section className={styles.page} aria-labelledby="locations-title">
      <header className={styles.header}>
        <p className={styles.eyebrow}>Catálogo</p>
        <h1 id="locations-title">Localizações</h1>
        <p>Conheça as localizações cadastradas na Big Bang API.</p>
      </header>

      {isLoading && <p className={styles.status} role="status">Carregando localizações...</p>}

      {errorMessage && (
        <div className={`${styles.status} ${styles.error}`} role="alert">
          <p>{errorMessage}</p>
          <button onClick={() => setReloadKey((key) => key + 1)} type="button">Tentar novamente</button>
        </div>
      )}

      {isEmpty && <p className={styles.status}>Nenhuma localização foi encontrada.</p>}

      {!isLoading && !errorMessage && locations.length > 0 && (
        <div className={styles.grid}>
          {locations.map((location) => (
            <article className={styles.card} key={location.id}>
              <div className={styles.marker} aria-hidden="true">📍</div>
              <div className={styles.content}>
                <h2>{location.name}</h2>
                <h3>Personagens associados</h3>
                {location.characters.length > 0 ? (
                  <ul>
                    {location.characters.map((character) => <li key={character.id}>{character.name}</li>)}
                  </ul>
                ) : (
                  <p className={styles.emptyCharacters}>Nenhum personagem associado.</p>
                )}
              </div>
            </article>
          ))}
        </div>
      )}
    </section>
  )
}

export default LocationsPage
