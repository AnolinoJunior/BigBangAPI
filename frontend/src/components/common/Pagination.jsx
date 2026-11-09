import styles from './Pagination.module.css'

function Pagination({
  currentPage,
  totalPages,
  hasNextPage,
  hasPreviousPage,
  isLoading,
  onPageChange,
}) {
  const canGoToPreviousPage = (hasPreviousPage ?? currentPage > 1) && !isLoading
  const canGoToNextPage = hasNextPage && !isLoading

  return (
    <nav className={styles.pagination} aria-label="Paginacao de personagens">
      <button className={styles.button} disabled={!canGoToPreviousPage} onClick={() => onPageChange(currentPage - 1)} type="button">
        Anterior
      </button>
      <span className={styles.currentPage} aria-live="polite">
        Pagina {currentPage}{totalPages ? ` de ${totalPages}` : ''}
      </span>
      <button className={styles.button} disabled={!canGoToNextPage} onClick={() => onPageChange(currentPage + 1)} type="button">
        Proxima
      </button>
    </nav>
  )
}

export default Pagination
