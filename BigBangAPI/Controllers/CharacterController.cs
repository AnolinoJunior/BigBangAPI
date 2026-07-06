using BigBangAPI.Context;
using BigBangAPI.DTO;
using BigBangAPI.Filters;
using BigBangAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CharacterController> _logger;

        public CharacterController(AppDbContext context, ILogger<CharacterController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Character character)
        {
            _logger.LogInformation("Creating a new character: {CharacterName}", character.Name);

            if (await _context.Characters.AnyAsync(c => c.Name == character.Name))
            {
                return BadRequest("Character already exists");
            }

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            await _context.Entry(character)
        .Reference(c => c.Location)
        .LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = character.Id }, character);
        }
        
       [HttpGet]
[ServiceFilter(typeof(ApiLoggingFilter))]
public async Task<IActionResult> Get(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
{
    if (page < 1)
        return BadRequest("Page must be greater than 0.");

    if (pageSize < 1 || pageSize > 50)
        return BadRequest("PageSize must be between 1 and 50.");

    var characters = await _context.Characters
        .AsNoTracking()
        .Include(c => c.Location)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return Ok(characters);
}

      [HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var character = await _context.Characters
        .Include(c => c.Location)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (character == null)
    {
        return NotFound(new
        {
            Message = $"Character with ID {id} was not found."
        });
    }

    return Ok(character);
}
        


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Character updatedCharacter)
        {
            if (id != updatedCharacter.Id)
                return BadRequest();

            var character = await _context.Characters.FindAsync(id);

            if (character == null)
                return NotFound();

            character.Name = updatedCharacter.Name;
            character.Actor = updatedCharacter.Actor;
            character.Occupation = updatedCharacter.Occupation;
            character.University = updatedCharacter.University;
            character.IQ = updatedCharacter.IQ;
            character.Age = updatedCharacter.Age;
            character.LocationId = updatedCharacter.LocationId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            _logger.LogInformation("Deleting character with ID: {CharacterId}", id);

            var character = await _context.Characters.FindAsync(id);

            if (character == null)
                return NotFound();

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("dto")]
            public async Task<ActionResult<IEnumerable<CharacterDTO>>> GetCharacters([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (pageSize < 1 || pageSize > 50)
            {
                return BadRequest("PageSize must be between 1 and 50.");
            }

            var characters = await _context.Characters
            .Select(f => new CharacterDTO
        {
            Id = f.Id,
            Name = f.Name,
            Actor = f.Actor,
            Occupation = f.Occupation
        })
        .ToListAsync();

            return Ok(characters);
        }
        [HttpGet("{id:int:min(1)}/episodes")]
        public async Task<IActionResult> GetCharacterEpisodes(int id)
    {
        var episodes = await _context.CharacterEpisodes
            .Where(ce => ce.CharacterId == id)
            .Select(ce => ce.Episode)
            .ToListAsync();

        return Ok(episodes);
}

    }
}