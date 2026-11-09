import { useEffect, useState } from 'react'

const emptyEpisode = {
  title: '',
  season: 1,
  episodeNumber: 1,
  airDate: '',
}

function getInitialFormData(episode) {
  if (!episode) {
    return emptyEpisode
  }

  return {
    title: episode.title ?? '',
    season: episode.season ?? 1,
    episodeNumber: episode.episodeNumber ?? 1,
    airDate: '',
  }
}

function EpisodeFormModal({ episode, errorMessage, isSaving, onCancel, onSave }) {
  const [formData, setFormData] = useState(() => getInitialFormData(episode))
  const [validationMessage, setValidationMessage] = useState('')
  const isEditing = Boolean(episode)

  useEffect(() => {
    setFormData(getInitialFormData(episode))
    setValidationMessage('')
  }, [episode])

  function handleChange(event) {
    const { name, value } = event.target
    setFormData((currentData) => ({ ...currentData, [name]: value }))
  }

  function handleSubmit(event) {
    event.preventDefault()

    if (!formData.title.trim() || !formData.airDate) {
      setValidationMessage('Preencha todos os campos obrigatórios.')
      return
    }

    onSave({
      ...formData,
      ...(isEditing && { id: episode.id }),
      season: Number(formData.season),
      episodeNumber: Number(formData.episodeNumber),
    })
  }

  return (
    <>
      <div aria-labelledby="episode-form-title" aria-modal="true" className="modal d-block" role="dialog" tabIndex="-1">
        <div className="modal-dialog modal-dialog-centered">
          <div className="modal-content">
            <form onSubmit={handleSubmit}>
              <div className="modal-header">
                <h2 className="modal-title fs-5" id="episode-form-title">
                  {isEditing ? 'Editar Episode' : 'Novo Episode'}
                </h2>
                <button aria-label="Fechar" className="btn-close" disabled={isSaving} onClick={onCancel} type="button" />
              </div>
              <div className="modal-body">
                {validationMessage && <div className="alert alert-danger" role="alert">{validationMessage}</div>}
                {errorMessage && <div className="alert alert-danger" role="alert">{errorMessage}</div>}
                {isEditing && (
                  <div className="alert alert-info">
                    A API não retorna a data de exibição do episódio. Informe-a novamente para salvar a edição.
                  </div>
                )}
                <div className="row g-3">
                  <div className="col-12">
                    <label className="form-label" htmlFor="episode-title">Título *</label>
                    <input className="form-control" id="episode-title" maxLength="45" name="title" onChange={handleChange} required value={formData.title} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label" htmlFor="episode-season">Temporada *</label>
                    <input className="form-control" id="episode-season" max="12" min="1" name="season" onChange={handleChange} required type="number" value={formData.season} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label" htmlFor="episode-number">Número do episódio</label>
                    <input className="form-control" id="episode-number" min="1" name="episodeNumber" onChange={handleChange} type="number" value={formData.episodeNumber} />
                  </div>
                  <div className="col-12">
                    <label className="form-label" htmlFor="episode-air-date">Data de exibição *</label>
                    <input className="form-control" id="episode-air-date" name="airDate" onChange={handleChange} required type="date" value={formData.airDate} />
                  </div>
                </div>
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

export default EpisodeFormModal
