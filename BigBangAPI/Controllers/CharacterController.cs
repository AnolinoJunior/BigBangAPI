using AutoMapper;
using BigBangAPI.DTO;
using BigBangAPI.Filters;
using BigBangAPI.Models;
using BigBangAPI.Pagination;
using BigBangAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        /// <summary>Cadastra um personagem.</summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<CharacterDTO>> Create([FromBody] CharacterCreateDTO dto)
        {
            if (dto is null)
            {
                _logger.LogWarning("Foi recebida uma solicitação de criação com um personagem nulo.");
                return BadRequest();
            }

            _logger.LogInformation(
                "Criando o personagem {CharacterName}.",
                dto.Name);

            var character = _mapper.Map<Character>(dto);

            var novoCharacter = _uof.CharacterRepository.Create(character);

            await _uof.CommitAsync();

            var characterDto = _mapper.Map<CharacterDTO>(novoCharacter);

            return CreatedAtAction(
                nameof(GetById),
                new { id = novoCharacter.Id },
                characterDto);
        }

        /// <summary>Lista os personagens com paginação.</summary>
        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<CharacterDTO>>> Get(
            [FromQuery] CharacterParameters characterParams)
        {
            var characters = await _uof.CharacterRepository.GetCharactersAsync(characterParams);

            if (!characters.Any())
            {
                return NotFound(new
                {
                    Message = "Nenhum personagem encontrado."
                });
            }

            var metadata = new
            {
                characters.TotalCount,
                characters.PageSize,
                characters.CurrentPage,
                characters.TotalPages,
                characters.HasNext,
                characters.HasPrevious
            };

            Response.Headers.Append(
                "X-Pagination",
                JsonSerializer.Serialize(metadata));

            var charactersDto = _mapper.Map<IEnumerable<CharacterDTO>>(characters);

            return Ok(charactersDto);
        }

        /// <summary>Consulta os detalhes de um personagem pelo ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacterDetailsDTO>> GetById(int id)
        {
            var character = await _uof.CharacterRepository.GetCharacterDetailsAsync(id);

            if (character == null)
            {
                return NotFound(new
                {
                    Message = $"O personagem com ID {id} não foi encontrado."
                });
            }

            var dto = _mapper.Map<CharacterDetailsDTO>(character);

            return Ok(dto);
        }

        /// <summary>Atualiza um personagem.</summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult<CharacterDTO>> Update(
            int id,
            [FromBody] CharacterUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                _logger.LogWarning("Foi recebida uma solicitação de atualização com IDs divergentes.");
                return BadRequest();
            }

            var character = await _uof.CharacterRepository.GetByIdAsync(c => c.Id == id);

            if (character == null)
            {
                return NotFound(new
                {
                    Message = $"O personagem com ID {id} não foi encontrado."
                });
            }

            _mapper.Map(dto, character);

            _uof.CharacterRepository.Update(character);

            await _uof.CommitAsync();

            var characterDto = _mapper.Map<CharacterDTO>(character);

            return Ok(characterDto);
        }

        /// <summary>Exclui um personagem.</summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<CharacterDTO>> Delete(int id)
        {
            var character = await _uof.CharacterRepository.GetByIdAsync(c => c.Id == id);

            if (character == null)
            {
                _logger.LogWarning(
                    "Foi solicitada a exclusão do personagem inexistente com ID {CharacterId}.",
                    id);

                return NotFound(new
                {
                    Message = $"O personagem com ID {id} não foi encontrado."
                });
            }

            _uof.CharacterRepository.Delete(character);

            await _uof.CommitAsync();

            var dto = _mapper.Map<CharacterDTO>(character);

            return Ok(dto);
        }

        /// <summary>Lista os episódios em que o personagem aparece.</summary>
        [HttpGet("{id:int:min(1)}/episodes")]
        public async Task<ActionResult<IEnumerable<EpisodeDTO>>> GetCharacterEpisodes(int id)
        {
            var episodes = await _uof.CharacterRepository.GetEpisodesByCharacterAsync(id);

            if (!episodes.Any())
            {
                return NotFound(new
                {
                    Message = $"Nenhum episódio encontrado para o personagem {id}."
                });
            }

            var episodesDto = _mapper.Map<IEnumerable<EpisodeDTO>>(episodes);

            return Ok(episodesDto);
        }

        /// <summary>Atualiza campos específicos de um personagem com JSON Patch.</summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<CharacterDTO>> Patch(
            int id,
            [FromBody] JsonPatchDocument<CharacterUpdateDTO> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var character = await _uof.CharacterRepository.GetByIdAsync(c => c.Id == id);

            if (character == null)
            {
                return NotFound(new
                {
                    Message = $"O personagem com ID {id} não foi encontrado."
                });
            }

            var characterToPatch = _mapper.Map<CharacterUpdateDTO>(character);

            patchDoc.ApplyTo(characterToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(characterToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(characterToPatch, character);

            _uof.CharacterRepository.Update(character);

            await _uof.CommitAsync();

            var characterDto = _mapper.Map<CharacterDTO>(character);

            return Ok(characterDto);
        }
    }
}