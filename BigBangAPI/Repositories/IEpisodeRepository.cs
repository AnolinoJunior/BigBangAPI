using BigBangAPI.Models;
using BigBangAPI.Pagination;

namespace BigBangAPI.Repositories
{
    public interface IEpisodeRepository : IRepository<Episode>
    {
        IEnumerable<Episode> GetEpisodes(EpisodeParameters episodeParams);

        IEnumerable<Character> GetCharactersByEpisode(int id);
    }
}