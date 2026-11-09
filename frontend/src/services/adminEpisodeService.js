import api from './api'

export async function createEpisode(episode) {
  const response = await api.post('/Episode', episode)
  return response.data
}

export async function updateEpisode(id, episode) {
  const response = await api.put(`/Episode/${id}`, episode)
  return response.data
}

export async function deleteEpisode(id) {
  await api.delete(`/Episode/${id}`)
}
