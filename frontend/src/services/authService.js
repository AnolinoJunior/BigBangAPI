import api from './api'

export async function login({ userName, password }) {
  const response = await api.post('/Auth/login', {
    userName,
    password,
  })

  return response.data
}
