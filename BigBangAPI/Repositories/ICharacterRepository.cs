using BigBangAPI.DTO;
using BigBangAPI.Models;
namespace BigBangAPI.Repositories
{
    public interface ICharacterRepository
    {
        IEnumerable<Character> GetCharacters();
        Character? GetCharacter(int id);
        IEnumerable<Episode> GetEpisodesByCharacter(int id);
        Character Create(Character character);
        Character? Update(Character character);
        Character? Delete(int id);
    }
}