import api from './api'

export async function uploadCharacterImage(file) {
  const formData = new FormData()
  formData.append('file', file)

  const response = await api.post('/Upload', formData)
  const imageUrl = response.data.imageUrl ?? response.data.ImageUrl

  if (!imageUrl) {
    throw new Error('A API não retornou a URL da imagem enviada.')
  }

  return imageUrl
}
