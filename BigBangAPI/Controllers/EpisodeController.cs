using BigBangAPI.Models;
using BigBangAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EpisodeController : ControllerBase
    {
        private readonly IEpisodeRepository _repository;

        public EpisodeController(IEpisodeRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public ActionResult<Episode> Create([FromBody] Episode episode)
        {
            if (episode == null)
            {
                return BadRequest();
            }

            var newEpisode = _repository.Create(episode);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newEpisode.Id },
                newEpisode);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Episode>> Get()
        {
            var episodes = _repository.GetEpisodes();

            return Ok(episodes);
        }

        [HttpGet("{id}")]
        public ActionResult<Episode> GetById(int id)
        {
            var episode = _repository.GetEpisode(id);

            if (episode == null)
            {
                return NotFound(new
                {
                    Message = $"Episode with ID {id} was not found."
                });
            }

            return Ok(episode);
        }

        [HttpPut("{id}")]
        public ActionResult<Episode> Update(int id, Episode episode)
        {
            if (id != episode.Id)
            {
                return BadRequest();
            }

            var updatedEpisode = _repository.Update(episode);

            if (updatedEpisode == null)
            {
                return NotFound(new
                {
                    Message = $"Episode with ID {id} was not found."
                });
            }

            return Ok(updatedEpisode);
        }

        [HttpDelete("{id}")]
        public ActionResult<Episode> Delete(int id)
        {
            var episode = _repository.Delete(id);

            if (episode == null)
            {
                return NotFound(new
                {
                    Message = $"Episode with ID {id} was not found."
                });
            }

            return Ok(episode);
        }

        [HttpGet("{id:int:min(1)}/characters")]
        public ActionResult<IEnumerable<Character>> GetCharactersByEpisode(int id)
        {
            var characters = _repository.GetCharactersByEpisode(id);

            if (!characters.Any())
            {
                return NotFound(new
                {
                    Message = $"No characters were found for episode {id}."
                });
            }

            return Ok(characters);
        }
    }
}