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

        public EpisodeController(IUnityOfWork uof)
        {
            _uof = uof;
        }

        [HttpPost]
        public ActionResult<Episode> Create([FromBody] Episode episode)
        {
            if (episode == null)
            {
                return BadRequest();
            }

            var newEpisode = _uof.EpisodeRepository.Create(episode);
            _uof.Commit();

            return CreatedAtAction(
                nameof(GetById),
                new { id = newEpisode.Id },
                newEpisode);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Episode>> Get()
        {
            var episodes = _uof.EpisodeRepository.GetAll();

            return Ok(episodes);
        }

        [HttpGet("{id}")]
        public ActionResult<Episode> GetById(int id)
        {
            var episode = _uof.EpisodeRepository.GetById(e => e.Id == id);

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

            var updatedEpisode = _uof.EpisodeRepository.Update(episode);
            _uof.Commit();

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

            return Ok(episode);
        }

        [HttpGet("{id:int:min(1)}/characters")]
        public ActionResult<IEnumerable<Character>> GetCharactersByEpisode(int id)
        {
            var characters = _uof.EpisodeRepository.GetCharactersByEpisode(id);

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