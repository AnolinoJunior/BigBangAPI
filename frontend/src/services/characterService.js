import api from './api'

export async function getCharacters({ pageNumber = 1, pageSize = 5, name = '' } = {}) {
  const response = await api.get('/Character', {
    params: {
      pageNumber,
      pageSize,
      ...(name.trim() && { Name: name.trim() }),
    },
  })

  const paginationHeader = response.headers['x-pagination']

  return {
    characters: response.data,
    pagination: paginationHeader ? JSON.parse(paginationHeader) : null,
  }
}

export async function getCharacterById(characterId) {
  const response = await api.get(`/Character/${characterId}`)

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
  const apiBaseUrl = new URL(api.defaults.baseURL, window.location.origin)

  return new URL(`/images/characters/${imageFileName}`, apiBaseUrl).toString()
}