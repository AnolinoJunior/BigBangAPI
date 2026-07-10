using BigBangAPI.Models;
namespace BigBangAPI.Repositories
{
    public interface ICharacterRepository : IRepository<Character>
    {
    IEnumerable<Episode> GetEpisodesByCharacter(int id);

    }
}