import api from './api'

export async function getCharacters({ pageNumber = 1, pageSize = 5 } = {}) {
  const response = await api.get('/Character', {
    params: {
      pageNumber,
      pageSize,
    },
  })

  return response.data
}

export function getCharacterImageUrl(imageUrl) {
  if (!imageUrl) {
    return null
  }

  if (/^https?:\/\//i.test(imageUrl)) {
    return imageUrl
  }

  const imageFileName = imageUrl.replace(/^\/?(?:images\/characters\/)?/i, '')

  return new URL(`/images/characters/${imageFileName}`, api.defaults.baseURL).toString()
}
