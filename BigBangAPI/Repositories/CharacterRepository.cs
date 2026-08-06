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

        public async Task<PagedList<Character>> GetCharactersAsync(CharacterParameters characterParams)
        {
            IQueryable<Character> characters = GetAll();

            if (!string.IsNullOrWhiteSpace(characterParams.Name))
            {
                characters = characters.Where(c =>
                    EF.Functions.Like(c.Name, $"%{characterParams.Name}%"));
            }

            characters = characters.OrderBy(c => c.Name);

            return await PagedList<Character>.ToPagedListAsync(
                characters,
                characterParams.PageNumber,
                characterParams.PageSize);
        }

        public async Task<Character?> GetCharacterDetailsAsync(int id)
        {
            return await _context.Characters
                .Include(c => c.Location)
                .Include(c => c.CharacterEpisodes!)
                    .ThenInclude(ce => ce.Episode)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Episode>> GetEpisodesByCharacterAsync(int id)
        {
            return await _context.CharacterEpisodes
                .Where(ce => ce.CharacterId == id)
                .Select(ce => ce.Episode)
                .Where(e => e != null)
                .Select(e => e!)
                .ToListAsync();
        }
    }
}