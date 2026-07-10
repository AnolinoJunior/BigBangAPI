using BigBangAPI.Context;
using BigBangAPI.Models;

namespace BigBangAPI.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(AppDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Character> GetCharactersByLocation(int id)
        {
            return _context.Characters
                .Where(c => c.LocationId == id)
                .ToList();
        }
    }
}