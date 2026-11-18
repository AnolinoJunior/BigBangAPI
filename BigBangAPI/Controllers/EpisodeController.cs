using AutoMapper;
using BigBangAPI.DTO;
using BigBangAPI.Models;
using BigBangAPI.Pagination;
using BigBangAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodeController : ControllerBase
    {
        private readonly IUnityOfWork _uof;
        private readonly IMapper _mapper;

        public EpisodeController(
            IUnityOfWork uof,
            IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        /// <summary>Cadastra um episódio.</summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<EpisodeDTO>> Create([FromBody] EpisodeCreateDTO dto)
        {
            if (dto is null)
            {
                return BadRequest();
            }

            var episode = _mapper.Map<Episode>(dto);

            var newEpisode = _uof.EpisodeRepository.Create(episode);

            await _uof.CommitAsync();

            var episodeDto = _mapper.Map<EpisodeDTO>(newEpisode);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newEpisode.Id },
                episodeDto);
        }

        /// <summary>Lista os episódios com paginação.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EpisodeDTO>>> Get(
            [FromQuery] EpisodeParameters episodeParams)
        {
            var episodes = await _uof.EpisodeRepository.GetEpisodesAsync(episodeParams);

            if (!episodes.Any())
            {
                return NotFound(new
                {
                    Message = "Nenhum episódio encontrado."
                });
            }

            var episodesDto = _mapper.Map<IEnumerable<EpisodeDTO>>(episodes);

            return Ok(episodesDto);
        }

        /// <summary>Consulta um episódio pelo ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EpisodeDTO>> GetById(int id)
        {
            var episode = await _uof.EpisodeRepository.GetByIdAsync(e => e.Id == id);

            if (episode == null)
            {
                return NotFound(new
                {
                    Message = $"O episódio com ID {id} não foi encontrado."
                });
            }

            var dto = _mapper.Map<EpisodeDTO>(episode);

            return Ok(dto);
        }

        /// <summary>Atualiza um episódio.</summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult<EpisodeDTO>> Update(
            int id,
            [FromBody] EpisodeUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var episode = await _uof.EpisodeRepository.GetByIdAsync(e => e.Id == id);

            if (episode == null)
            {
                return NotFound(new
                {
                    Message = $"O episódio com ID {id} não foi encontrado."
                });
            }

            _mapper.Map(dto, episode);

            _uof.EpisodeRepository.Update(episode);

            await _uof.CommitAsync();

            var episodeDto = _mapper.Map<EpisodeDTO>(episode);

            return Ok(episodeDto);
        }

        /// <summary>Exclui um episódio.</summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<EpisodeDTO>> Delete(int id)
        {
            var episode = await _uof.EpisodeRepository.GetByIdAsync(e => e.Id == id);

            if (episode == null)
            {
                return NotFound(new
                {
                    Message = $"O episódio com ID {id} não foi encontrado."
                });
            }

            _uof.EpisodeRepository.Delete(episode);

            await _uof.CommitAsync();

            var dto = _mapper.Map<EpisodeDTO>(episode);

            return Ok(dto);
        }

        /// <summary>Lista os personagens que aparecem no episódio.</summary>
        [HttpGet("{id:int:min(1)}/characters")]
        public async Task<ActionResult<IEnumerable<CharacterDTO>>> GetCharactersByEpisode(int id)
        {
            var characters = await _uof.EpisodeRepository.GetCharactersByEpisodeAsync(id);

            if (!characters.Any())
            {
                return NotFound(new
                {
                    Message = $"Nenhum personagem encontrado para o episódio {id}."
                });
            }

            var charactersDto = _mapper.Map<IEnumerable<CharacterDTO>>(characters);

            return Ok(charactersDto);
        }
    }
}