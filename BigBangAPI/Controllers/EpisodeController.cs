using BigBangAPI.Context;
using BigBangAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EpisodeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Episode episode)
        {
            _context.Episodes.Add(episode);

            await _context.SaveChangesAsync();

            return Ok(episode);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var episodes = await _context.Episodes
                .ToListAsync();

            return Ok(episodes);
        }
    }
}