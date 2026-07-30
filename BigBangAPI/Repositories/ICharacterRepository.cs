using BigBangAPI.Models;
using BigBangAPI.Pagination;

namespace BigBangAPI.Repositories;

public interface ICharacterRepository : IRepository<Character>
{
    PagedList<Character> GetCharacters(CharacterParameters characterParams);

    Character? GetCharacterDetails(int id);

    IEnumerable<Episode> GetEpisodesByCharacter(int id);
}