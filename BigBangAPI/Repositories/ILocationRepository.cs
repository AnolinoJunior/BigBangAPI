using BigBangAPI.Models;

namespace BigBangAPI.Repositories
{
   public interface ILocationRepository : IRepository<Location>
{
    IEnumerable<Character> GetCharactersByLocation(int id);
}
}