using AutoMapper;
using BigBangAPI.DTO;
using BigBangAPI.Models;
using BigBangAPI.Repositories;
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

        [HttpPost]
        public ActionResult<EpisodeDTO> Create([FromBody] EpisodeCreateDTO dto)
        {
            if (dto is null)
            {
                return BadRequest();
            }

            var episode = _mapper.Map<Episode>(dto);

            var newEpisode = _uof.EpisodeRepository.Create(episode);

            _uof.Commit();

            var episodeDto = _mapper.Map<EpisodeDTO>(newEpisode);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newEpisode.Id },
                episodeDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<EpisodeDTO>> Get()
        {
            var episodes = _uof.EpisodeRepository.GetAll();

            var episodesDto = _mapper.Map<IEnumerable<EpisodeDTO>>(episodes);

            return Ok(episodesDto);
        }

        [HttpGet("{id}")]
        public ActionResult<EpisodeDTO> GetById(int id)
        {
            var episode = _uof.EpisodeRepository.GetById(e => e.Id == id);

            if (episode == null)
            {
                return NotFound(new
                {
                    Message = $"Episode with ID {id} was not found."
                });
            }

            var dto = _mapper.Map<EpisodeDTO>(episode);

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public ActionResult<EpisodeDTO> Update(int id, [FromBody] EpisodeUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var episode = _mapper.Map<Episode>(dto);

            var updatedEpisode = _uof.EpisodeRepository.Update(episode);

            if (updatedEpisode == null)
            {
                return NotFound(new
                {
                    Message = $"Episode with ID {id} was not found."
                });
            }

            _uof.Commit();

            var episodeDto = _mapper.Map<EpisodeDTO>(updatedEpisode);

            return Ok(episodeDto);
        }

        [HttpDelete("{id}")]
        public ActionResult<EpisodeDTO> Delete(int id)
        {
            var episode = _uof.EpisodeRepository.GetById(e => e.Id == id);

            if (episode == null)
            {
                return NotFound(new
                {
                    Message = $"Episode with ID {id} was not found."
                });
            }

            _uof.EpisodeRepository.Delete(episode);

            _uof.Commit();

            var dto = _mapper.Map<EpisodeDTO>(episode);

            return Ok(dto);
        }

        [HttpGet("{id:int:min(1)}/characters")]
        public ActionResult<IEnumerable<CharacterDTO>> GetCharactersByEpisode(int id)
        {
            var characters = _uof.EpisodeRepository.GetCharactersByEpisode(id);

            if (!characters.Any())
            {
                return NotFound(new
                {
                    Message = $"No characters were found for episode {id}."
                });
            }

            var charactersDto = _mapper.Map<IEnumerable<CharacterDTO>>(characters);

            return Ok(charactersDto);
        }
    }
}