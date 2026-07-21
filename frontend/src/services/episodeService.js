import api from './api'

export async function getEpisodes({ pageNumber = 1, pageSize = 10 } = {}) {
  const response = await api.get('/Episode', {
    params: {
      pageNumber,
      pageSize,
    },
  })

  return response.data
}

const MAX_PAGE_SIZE = 40

export async function getAllEpisodes() {
  const episodes = []
  let pageNumber = 1

  while (true) {
    try {
      const episodesPage = await getEpisodes({
        pageNumber,
        pageSize: MAX_PAGE_SIZE,
      })

      episodes.push(...episodesPage)

      if (episodesPage.length < MAX_PAGE_SIZE) {
        return episodes
      }

      pageNumber += 1
    } catch (error) {
      if (error.response?.status === 404) {
        return episodes
      }

      throw error
    }
  }
}
