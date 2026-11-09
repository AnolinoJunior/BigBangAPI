import styles from './SearchBar.module.css'

function SearchBar({ value, onChange, onClear }) {
  return (
    <div className={styles.search}>
      <label className={styles.label} htmlFor="character-search">Buscar personagem</label>
      <div className={styles.controls}>
        <span aria-hidden="true" className={styles.icon}>🔍</span>
        <input id="character-search" onChange={(event) => onChange(event.target.value)} placeholder="Buscar personagem..." type="search" value={value} />
        {value && <button onClick={onClear} type="button">Limpar</button>}
      </div>
    </div>
  )
}

export default SearchBar
