using BigBangAPI.Context;
using BigBangAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBangAPI.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(AppDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Character>> GetCharactersByLocationAsync(int id)
        {
            return await _context.Characters
                .Where(c => c.LocationId == id)
                .ToListAsync();
        }
    }
}