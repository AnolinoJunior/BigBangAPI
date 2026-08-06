using BigBangAPI.Models;

namespace BigBangAPI.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<IEnumerable<Character>> GetCharactersByLocationAsync(int id);
    }
}