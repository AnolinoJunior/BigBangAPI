import axios from 'axios'

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5004/api',
  headers: {
    Accept: 'application/json',
  },
  timeout: 10000,
})

api.interceptors.request.use((config) => {
  const accessToken = sessionStorage.getItem('bigbangapi.accessToken')

  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`
  }

  return config
})

export default api
