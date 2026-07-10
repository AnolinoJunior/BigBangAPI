using BigBangAPI.Context;
using BigBangAPI.Models;

namespace BigBangAPI.Repositories
{
    public class EpisodeRepository : Repository<Episode>, IEpisodeRepository
    {
        public EpisodeRepository(AppDbContext context)
            : base(context)
        {
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