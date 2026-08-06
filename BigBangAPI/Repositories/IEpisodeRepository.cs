using BigBangAPI.Models;
using BigBangAPI.Pagination;

namespace BigBangAPI.Repositories
{
    public interface IEpisodeRepository : IRepository<Episode>
    {
        Task<IEnumerable<Episode>> GetEpisodesAsync(EpisodeParameters episodeParams);

        Task<IEnumerable<Character>> GetCharactersByEpisodeAsync(int id);
    }
}