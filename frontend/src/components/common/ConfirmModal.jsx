function ConfirmModal({ isSubmitting, message, onCancel, onConfirm, title }) {
  return (
    <>
      <div aria-labelledby="confirm-modal-title" aria-modal="true" className="modal d-block" role="dialog" tabIndex="-1">
        <div className="modal-dialog modal-dialog-centered">
          <div className="modal-content">
            <div className="modal-header">
              <h2 className="modal-title fs-5" id="confirm-modal-title">{title}</h2>
              <button aria-label="Fechar" className="btn-close" disabled={isSubmitting} onClick={onCancel} type="button" />
            </div>
            <div className="modal-body"><p className="mb-0">{message}</p></div>
            <div className="modal-footer">
              <button className="btn btn-secondary" disabled={isSubmitting} onClick={onCancel} type="button">Cancelar</button>
              <button className="btn btn-danger" disabled={isSubmitting} onClick={onConfirm} type="button">
                {isSubmitting ? 'Excluindo...' : 'Excluir'}
              </button>
            </div>
          </div>
        </div>
      </div>
      <div className="modal-backdrop show" />
    </>
  )
}

export default ConfirmModal
