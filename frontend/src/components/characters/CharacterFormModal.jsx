import { useEffect, useState } from 'react'
import { getCharacterImageUrl } from '../../services/characterService'

const emptyCharacter = {
  name: '',
  actor: '',
  occupation: '',
  university: '',
  iq: 0,
  age: 0,
  locationId: '',
  imageUrl: '',
}

function getInitialFormData(character, locations) {
  if (!character) {
    return emptyCharacter
  }

  const location = locations.find((item) => item.name === character.locationName)

  return {
    name: character.name ?? '',
    actor: character.actor ?? '',
    occupation: character.occupation ?? '',
    university: character.university ?? '',
    iq: character.iq ?? 0,
    age: character.age ?? 0,
    locationId: location?.id ?? '',
    imageUrl: character.imageUrl ?? '',
  }
}

const acceptedImageExtensions = ['.jpg', '.jpeg', '.png', '.webp']

function CharacterFormModal({ character, errorMessage, isSaving, isUploading, locations, onCancel, onSave }) {
  const [formData, setFormData] = useState(() => getInitialFormData(character, locations))
  const [validationMessage, setValidationMessage] = useState('')
  const [selectedImage, setSelectedImage] = useState(null)
  const [previewUrl, setPreviewUrl] = useState(null)
  const isEditing = Boolean(character)
  const isBusy = isSaving || isUploading

  useEffect(() => {
    setFormData(getInitialFormData(character, locations))
    setValidationMessage('')
    setSelectedImage(null)
    setPreviewUrl(null)
  }, [character, locations])

  useEffect(() => () => {
    if (previewUrl) {
      URL.revokeObjectURL(previewUrl)
    }
  }, [previewUrl])

  function handleChange(event) {
    const { name, value } = event.target
    setFormData((currentData) => ({ ...currentData, [name]: value }))
  }

  function handleImageChange(event) {
    const file = event.target.files?.[0]

    if (!file) {
      setSelectedImage(null)
      setPreviewUrl(null)
      return
    }

    const extension = `.${file.name.split('.').pop()?.toLowerCase()}`

    if (!acceptedImageExtensions.includes(extension)) {
      setValidationMessage('Selecione uma imagem nos formatos JPG, JPEG, PNG ou WEBP.')
      setSelectedImage(null)
      setPreviewUrl(null)
      event.target.value = ''
      return
    }

    setValidationMessage('')
    setSelectedImage(file)
    setPreviewUrl(URL.createObjectURL(file))
  }

  function handleSubmit(event) {
    event.preventDefault()

    if (!formData.name.trim() || !formData.actor.trim() || !formData.occupation.trim() || !formData.university.trim() || !formData.locationId) {
      setValidationMessage('Preencha todos os campos obrigatórios.')
      return
    }

    onSave({
      characterData: {
        ...formData,
        ...(isEditing && { id: character.id }),
        iq: Number(formData.iq),
        age: Number(formData.age),
        locationId: Number(formData.locationId),
        imageUrl: formData.imageUrl || null,
      },
      imageFile: selectedImage,
    })
  }

  const displayedImageUrl = previewUrl ?? getCharacterImageUrl(formData.imageUrl)

  return (
    <>
      <div aria-labelledby="character-form-title" aria-modal="true" className="modal d-block" role="dialog" tabIndex="-1">
        <div className="modal-dialog modal-lg modal-dialog-scrollable">
          <div className="modal-content">
            <form onSubmit={handleSubmit}>
              <div className="modal-header">
                <h2 className="modal-title fs-5" id="character-form-title">
                  {isEditing ? 'Editar Character' : 'Novo Character'}
                </h2>
                <button aria-label="Fechar" className="btn-close" disabled={isBusy} onClick={onCancel} type="button" />
              </div>
              <div className="modal-body">
                {validationMessage && <div className="alert alert-danger" role="alert">{validationMessage}</div>}
                {errorMessage && <div className="alert alert-danger" role="alert">{errorMessage}</div>}
                <div className="row g-3">
                  <div className="col-md-6">
                    <label className="form-label" htmlFor="character-name">Nome *</label>
                    <input className="form-control" disabled={isBusy} id="character-name" maxLength="45" name="name" onChange={handleChange} required value={formData.name} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label" htmlFor="character-actor">Ator *</label>
                    <input className="form-control" disabled={isBusy} id="character-actor" maxLength="45" name="actor" onChange={handleChange} required value={formData.actor} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label" htmlFor="character-occupation">Ocupação *</label>
                    <input className="form-control" disabled={isBusy} id="character-occupation" maxLength="45" minLength="10" name="occupation" onChange={handleChange} required value={formData.occupation} />
                  </div>
                  <div className="col-md-6">
                    <label className="form-label" htmlFor="character-university">Universidade *</label>
                    <input className="form-control" disabled={isBusy} id="character-university" maxLength="45" name="university" onChange={handleChange} required value={formData.university} />
                  </div>
                  <div className="col-md-4">
                    <label className="form-label" htmlFor="character-iq">QI</label>
                    <input className="form-control" disabled={isBusy} id="character-iq" min="0" name="iq" onChange={handleChange} type="number" value={formData.iq} />
                  </div>
                  <div className="col-md-4">
                    <label className="form-label" htmlFor="character-age">Idade</label>
                    <input className="form-control" disabled={isBusy} id="character-age" min="0" name="age" onChange={handleChange} type="number" value={formData.age} />
                  </div>
                  <div className="col-md-4">
                    <label className="form-label" htmlFor="character-location">Localização *</label>
                    <select className="form-select" disabled={isBusy} id="character-location" name="locationId" onChange={handleChange} required value={formData.locationId}>
                      <option value="">Selecione</option>
                      {locations.map((location) => <option key={location.id} value={location.id}>{location.name}</option>)}
                    </select>
                  </div>
                  <div className="col-12">
                    <label className="form-label" htmlFor="character-image">Imagem</label>
                    <input accept=".jpg,.jpeg,.png,.webp,image/jpeg,image/png,image/webp" className="form-control" disabled={isBusy} id="character-image" onChange={handleImageChange} type="file" />
                    <div className="form-text">Formatos aceitos: JPG, JPEG, PNG e WEBP.</div>
                    {displayedImageUrl && (
                      <img alt="Preview do character" className="img-thumbnail mt-3" height="160" src={displayedImageUrl} style={{ objectFit: 'cover', width: '160px' }} />
                    )}
                  </div>
                </div>
              </div>
              <div className="modal-footer">
                <button className="btn btn-secondary" disabled={isBusy} onClick={onCancel} type="button">Cancelar</button>
                <button className="btn btn-primary" disabled={isBusy} type="submit">
                  {isUploading ? 'Enviando imagem...' : isSaving ? 'Salvando...' : 'Salvar'}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
      <div className="modal-backdrop show" />
    </>
  )
}

export default CharacterFormModal
