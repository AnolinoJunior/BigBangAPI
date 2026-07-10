using BigBangAPI.Context;
using BigBangAPI.Models;

namespace BigBangAPI.Repositories
{
    public class CharacterRepository : Repository<Character>, ICharacterRepository
    {
        public CharacterRepository(AppDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Episode> GetEpisodesByCharacter(int id)
        {
            return _context.CharacterEpisodes
                .Where(ce => ce.CharacterId == id)
                .Select(ce => ce.Episode)
                .Where(e => e != null)
                .Select(e => e!)
                .ToList();
        }
    }
}