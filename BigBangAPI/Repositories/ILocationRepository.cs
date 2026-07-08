using BigBangAPI.Models;

namespace BigBangAPI.Repositories
{
    public interface ILocationRepository
    {
        IEnumerable<Location> GetLocations();

        Location? GetLocation(int id);

        Location Create(Location location);

        Location? Update(Location location);

        Location? Delete(int id);
    }
}