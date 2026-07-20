using BigBangAPI.Context;
using BigBangAPI.Models;
using BigBangAPI.Pagination;
using Microsoft.EntityFrameworkCore;

namespace BigBangAPI.Repositories
{
    public class CharacterRepository : Repository<Character>, ICharacterRepository
    {
        public CharacterRepository(AppDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Character> GetCharacters(CharacterParameters characterParams)
        {
            return GetAll()
                .OrderBy(c => c.Name)
                .Skip((characterParams.PageNumber - 1) * characterParams.PageSize)
                .Take(characterParams.PageSize)
                .ToList();
        }

        public Character? GetCharacterDetails(int id)
        {
            return _context.Characters
                .Include(c => c.Location)
                .Include(c => c.CharacterEpisodes!)
                    .ThenInclude(ce => ce.Episode)
                .FirstOrDefault(c => c.Id == id);
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