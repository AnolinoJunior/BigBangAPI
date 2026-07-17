    using AutoMapper;
    using BigBangAPI.DTO;
    using BigBangAPI.Filters;
    using BigBangAPI.Models;
    using BigBangAPI.Repositories;
    using Microsoft.AspNetCore.Mvc;

    namespace BigBangAPI.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class CharacterController : ControllerBase
        {
            private readonly IUnityOfWork _uof; 
        
            private readonly IMapper _mapper;
            private readonly ILogger<CharacterController> _logger;

            public CharacterController(
                IUnityOfWork uof,
                IMapper mapper,
                ILogger<CharacterController> logger)
            {
                _uof = uof;
                _mapper = mapper;
                _logger = logger;
            }
            [HttpPost]
            public ActionResult<CharacterDTO> Create([FromBody] CharacterCreateDTO dto)
            {
                if (dto is null)
                {
                    _logger.LogWarning("Create request received with a null character.");
                    return BadRequest();
                }

                _logger.LogInformation(
                    "Creating a new character: {CharacterName}",
                    dto.Name);

                var character = _mapper.Map<Character>(dto);

                var novoCharacter = _uof.CharacterRepository.Create(character);

                _uof.Commit();

                var characterDto = _mapper.Map<CharacterDTO>(novoCharacter);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = novoCharacter.Id },
                    characterDto);
            }

            [HttpGet]
            [ServiceFilter(typeof(ApiLoggingFilter))]
            public ActionResult<IEnumerable<CharacterDTO>> Get()
            {
                var characters = _uof.CharacterRepository.GetAll();

                var charactersDto = _mapper.Map<IEnumerable<CharacterDTO>>(characters);

                return Ok(charactersDto);
            }

           [HttpGet("{id}")]
            public ActionResult<CharacterDetailsDTO> GetById(int id)
            {
                var character = _uof.CharacterRepository.GetById(c => c.Id == id);

                if (character == null)
                {
                    return NotFound(new
                    {
                        Message = $"Character with ID {id} was not found."
                    });
                }
                // var destino = _mapper.Map<Destino>(origem);
                var dto = _mapper.Map<CharacterDetailsDTO>(character);

                return Ok(dto);
            }

           [HttpPut("{id}")]
                public ActionResult<CharacterDTO> Update(int id, [FromBody] CharacterUpdateDTO dto)
                {
                    if (id != dto.Id)
                    {
                        _logger.LogWarning(
                            "Update request received with mismatched ID.");

                        return BadRequest();
                    }

                    var character = _mapper.Map<Character>(dto);

                    var updatedCharacter = _uof.CharacterRepository.Update(character);

                    if (updatedCharacter == null)
                    {
                        return NotFound(new
                        {
                            Message = $"Character with ID {id} was not found."
                        });
                    }

                    _uof.Commit();

                    var characterDto = _mapper.Map<CharacterDTO>(updatedCharacter);

                    return Ok(characterDto);
                }
            [HttpDelete("{id}")]
            public ActionResult<CharacterDTO> Delete(int id)
            {
                var character = _uof.CharacterRepository.GetById(c => c.Id == id);

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

                _uof.CharacterRepository.Delete(character);
                _uof.Commit();

                var dto = _mapper.Map<CharacterDTO>(character);

                return Ok(dto);
            }
           [HttpGet("{id:int:min(1)}/episodes")]
            public ActionResult<IEnumerable<EpisodeDTO>> GetCharacterEpisodes(int id)
            {
                var episodes = _uof.CharacterRepository.GetEpisodesByCharacter(id);

                if (!episodes.Any())
                {
                    return NotFound(new
                    {
                        Message = $"No episodes were found for character {id}."
                    });
                }

                var episodesDto = _mapper.Map<IEnumerable<EpisodeDTO>>(episodes);

                return Ok(episodesDto);
            }
        }
    }