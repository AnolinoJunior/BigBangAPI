import { useEffect, useState } from 'react'

function LocationFormModal({ errorMessage, isSaving, location, onCancel, onSave }) {
  const [name, setName] = useState(location?.name ?? '')
  const [validationMessage, setValidationMessage] = useState('')
  const isEditing = Boolean(location)

  useEffect(() => {
    setName(location?.name ?? '')
    setValidationMessage('')
  }, [location])

  function handleSubmit(event) {
    event.preventDefault()

    if (!name.trim()) {
      setValidationMessage('Informe o nome da localização.')
      return
    }

    onSave({
      ...(isEditing && { id: location.id }),
      name: name.trim(),
    })
  }

  return (
    <>
      <div aria-labelledby="location-form-title" aria-modal="true" className="modal d-block" role="dialog" tabIndex="-1">
        <div className="modal-dialog modal-dialog-centered">
          <div className="modal-content">
            <form onSubmit={handleSubmit}>
              <div className="modal-header">
                <h2 className="modal-title fs-5" id="location-form-title">
                  {isEditing ? 'Editar Location' : 'Nova Location'}
                </h2>
                <button aria-label="Fechar" className="btn-close" disabled={isSaving} onClick={onCancel} type="button" />
              </div>
              <div className="modal-body">
                {validationMessage && <div className="alert alert-danger" role="alert">{validationMessage}</div>}
                {errorMessage && <div className="alert alert-danger" role="alert">{errorMessage}</div>}
                <label className="form-label" htmlFor="location-name">Nome *</label>
                <input className="form-control" id="location-name" maxLength="45" onChange={(event) => setName(event.target.value)} required value={name} />
              </div>
              <div className="modal-footer">
                <button className="btn btn-secondary" disabled={isSaving} onClick={onCancel} type="button">Cancelar</button>
                <button className="btn btn-primary" disabled={isSaving} type="submit">{isSaving ? 'Salvando...' : 'Salvar'}</button>
              </div>
            </form>
          </div>
        </div>
      </div>
      <div className="modal-backdrop show" />
    </>
  )
}

export default LocationFormModal
