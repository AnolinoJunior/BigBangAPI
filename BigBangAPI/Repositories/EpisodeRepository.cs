using BigBangAPI.Context;
using BigBangAPI.Models;
using BigBangAPI.Pagination;

namespace BigBangAPI.Repositories
{
    public class EpisodeRepository : Repository<Episode>, IEpisodeRepository
    {
        public EpisodeRepository(AppDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Episode> GetEpisodes(EpisodeParameters episodeParams)
        {
            return GetAll()
                .OrderBy(e => e.Season)
                .ThenBy(e => e.EpisodeNumber)
                .Skip((episodeParams.PageNumber - 1) * episodeParams.PageSize)
                .Take(episodeParams.PageSize)
                .ToList();
        }

        public IEnumerable<Character> GetCharactersByEpisode(int id)
        {
            return _context.CharacterEpisodes
                .Where(ce => ce.EpisodeId == id)
                .Select(ce => ce.Character!)
                .ToList();
        }
    }
}