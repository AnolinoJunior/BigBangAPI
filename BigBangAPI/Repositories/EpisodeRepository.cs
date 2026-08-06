using BigBangAPI.Context;
using BigBangAPI.Models;
using BigBangAPI.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BigBangAPI.Repositories
{
    public class EpisodeRepository : Repository<Episode>, IEpisodeRepository
    {
        public EpisodeRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Episode>> GetEpisodesAsync(EpisodeParameters episodeParams)
        {
            return await GetAll()
                .OrderBy(e => e.Season)
                .ThenBy(e => e.EpisodeNumber)
                .Skip((episodeParams.PageNumber - 1) * episodeParams.PageSize)
                .Take(episodeParams.PageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Character>> GetCharactersByEpisodeAsync(int id)
        {
            return await _context.CharacterEpisodes
                .Where(ce => ce.EpisodeId == id)
                .Select(ce => ce.Character!)
                .ToListAsync();
        }
    }
}