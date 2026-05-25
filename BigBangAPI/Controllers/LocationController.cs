using BigBangAPI.Context;
using BigBangAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Locations.ToList());
        }

        [HttpPost]
        public IActionResult Create(Location location)
        {
            _context.Locations.Add(location);
            _context.SaveChanges();
            return Ok(location);
        }
    }
}