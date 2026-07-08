using BigBangAPI.Context;
using BigBangAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BigBangAPI.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _context;

        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Location> GetLocations()
        {
            return _context.Locations.ToList();
        }

        public Location? GetLocation(int id)
        {
            return _context.Locations.FirstOrDefault(l => l.Id == id);
        }

        public Location Create(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            _context.Locations.Add(location);
            _context.SaveChanges();

            return location;
        }

        public Location? Update(Location location)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            _context.Entry(location).State = EntityState.Modified;
            _context.SaveChanges();

            return location;
        }

        public Location? Delete(int id)
        {
            var location = _context.Locations.Find(id);

            if (location == null)
                return null;

            _context.Locations.Remove(location);
            _context.SaveChanges();

            return location;
        }
    }
}