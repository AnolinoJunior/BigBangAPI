using BigBangAPI.Context;
using BigBangAPI.DTO;
using BigBangAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CharacterController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Create(Character character)
        {
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
        public async Task<IActionResult> Get()
        {
            var characters = await _context.Characters
                .AsNoTracking()
                .Include(c => c.Location)
                .Skip(10)
                .Take(10)
                .ToListAsync();

            return Ok(characters);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An internal server error occurred.",
                    Error = ex.Message
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Character updatedCharacter)
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
            var character = await _context.Characters.FindAsync(id);

            if (character == null)
                return NotFound();

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("dto")]
        public ActionResult<IEnumerable<CharacterDTO>> GetCharacters()
        {
            var character = _context.Characters
                .Select(f => new CharacterDTO

                {
                    Id = f.Id,
                    Name = f.Name,
                    Actor = f.Actor,
                    Occupation = f.Occupation
                })
                .ToList();
            return Ok(character);
        }

    }
}