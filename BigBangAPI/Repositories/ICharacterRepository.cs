using BigBangAPI.Models;
using BigBangAPI.Pagination;

namespace BigBangAPI.Repositories;

public interface ICharacterRepository : IRepository<Character>
{
    Task<PagedList<Character>> GetCharactersAsync(CharacterParameters characterParams);

    Task<Character?> GetCharacterDetailsAsync(int id);

    Task<IEnumerable<Episode>> GetEpisodesByCharacterAsync(int id);
}