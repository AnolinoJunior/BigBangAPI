import api from './api'

export async function getLocations() {
  const response = await api.get('/Location')
  return response.data
}

export async function getLocationById(id) {
  const response = await api.get(`/Location/${id}`)
  return response.data
}

export async function createLocation(location) {
  const response = await api.post('/Location', location)
  return response.data
}

export async function updateLocation(id, location) {
  const response = await api.put(`/Location/${id}`, location)
  return response.data
}

export async function deleteLocation(id) {
  await api.delete(`/Location/${id}`)
}
