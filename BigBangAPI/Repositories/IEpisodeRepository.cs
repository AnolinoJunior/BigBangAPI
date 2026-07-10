using BigBangAPI.Models;

namespace BigBangAPI.Repositories
{
    public interface IEpisodeRepository : IRepository<Episode>
    {
        IEnumerable<Character> GetCharactersByEpisode(int id);
    }
}