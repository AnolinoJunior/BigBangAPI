import styles from './Pagination.module.css'

function Pagination({ currentPage, hasNextPage, isLoading, onPageChange }) {
  const canGoToPreviousPage = currentPage > 1 && !isLoading
  const canGoToNextPage = hasNextPage && !isLoading

  return (
    <nav className={styles.pagination} aria-label="Paginação de personagens">
      <button
        className={styles.button}
        disabled={!canGoToPreviousPage}
        onClick={() => onPageChange(currentPage - 1)}
        type="button"
      >
        Anterior
      </button>
      <span className={styles.currentPage} aria-live="polite">
        Página {currentPage}
      </span>
      <button
        className={styles.button}
        disabled={!canGoToNextPage}
        onClick={() => onPageChange(currentPage + 1)}
        type="button"
      >
        Próxima
      </button>
    </nav>
  )
}

export default Pagination
