    using BigBangAPI.DTO;
    using BigBangAPI.Filters;
    using BigBangAPI.Models;
    using BigBangAPI.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    namespace BigBangAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class CharacterController : ControllerBase
        {   
            private readonly ICharacterRepository _repository;
            private readonly ILogger<CharacterController> _logger;

            public CharacterController(ICharacterRepository repository, ILogger<CharacterController> logger)
            {
                _repository = repository;
                _logger = logger;
            }

 [HttpPost]
public ActionResult<Character> Create([FromBody] Character character)
{
    if (character is null)
    {
        _logger.LogWarning("Create request received with a null character.");

        return BadRequest();
    }

    _logger.LogInformation(
        "Creating a new character: {CharacterName}",
        character.Name);

    var novoCharacter = _repository.Create(character);

    return CreatedAtAction(
        nameof(GetById),
        new { id = novoCharacter.Id },
        novoCharacter);
}
    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult <IEnumerable<Character>> Get()
    {
        var characters = _repository.GetCharacters();

        return Ok(characters);
    }

        [HttpGet("{id}")]
    public ActionResult<Character> GetById(int id)
    {
        var character = _repository.GetCharacter(id);

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
public ActionResult<Character> Update(int id, Character character)
{
    if (id != character.Id)
    {
        _logger.LogWarning(
            "Update request received with mismatched ID: {CharacterId} and character ID: {CharacterObjectId}",
            id, character.Id);

        return BadRequest();
    }

    var updatedCharacter = _repository.Update(character);

    if (updatedCharacter == null)
    {
        return NotFound(new
        {
            Message = $"Character with ID {id} was not found."
        });
    }

    _logger.LogInformation(
        "Character with ID {CharacterId} updated successfully.",
        id);

    return Ok(updatedCharacter);
}

[HttpDelete("{id}")]
public ActionResult<Character> Delete(int id)
{
    var character = _repository.Delete(id);

    if (character == null)
    {
        _logger.LogWarning(
            "Delete request received for non-existent character with ID: {CharacterId}",
            id);

        return NotFound(new
        {
            Message = $"Character with ID {id} was not found."
        });
    }

    return Ok(character);
}

[HttpGet("dto")]

public ActionResult<IEnumerable<CharacterDTO>> GetCharacters()
{
    var characters = _repository.GetCharacters();

    var charactersDto = characters.Select(c => new CharacterDTO
    {
        Id = c.Id,
        Name = c.Name,
        Actor = c.Actor,
        Occupation = c.Occupation
    });

    return Ok(charactersDto);
}
    [HttpGet("{id:int:min(1)}/episodes")]
public ActionResult<IEnumerable<Episode>> GetCharacterEpisodes(int id)
{
    var episodes = _repository.GetEpisodesByCharacter(id);

    if (!episodes.Any())
    {
        return NotFound(new
        {
            Message = $"No episodes were found for character {id}."
        });
    }

    return Ok(episodes);
}

        }
    }