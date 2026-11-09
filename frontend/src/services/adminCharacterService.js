import api from './api'

export async function createCharacter(character) {
  const response = await api.post('/Character', character)
  return response.data
}

export async function updateCharacter(id, character) {
  const response = await api.put(`/Character/${id}`, character)
  return response.data
}

export async function deleteCharacter(id) {
  await api.delete(`/Character/${id}`)
}

export async function getLocationsForCharacterForm() {
  const response = await api.get('/Location')
  return response.data
}
