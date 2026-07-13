using BigBangAPI.Models;
using BigBangAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly IUnityOfWork _uof;

        public LocationController(IUnityOfWork uof)
        {
            _uof = uof;
        }
       

        [HttpGet]
        public ActionResult<IEnumerable<Location>> Get()
        {
            var locations = _uof.LocationRepository.GetAll();

            return Ok(locations);
        }

        [HttpGet("{id}")]
        public ActionResult<Location> GetById(int id)
        {
            var location = _uof.LocationRepository.GetById(l => l.Id == id);

            if (location == null)
            {
                return NotFound(new
                {
                    Message = $"Location with ID {id} was not found."
                });
            }

            return Ok(location);
        }

        [HttpPost]
        public ActionResult<Location> Create([FromBody] Location location)
        {
            if (location == null)
            {
                return BadRequest();
            }

            var newLocation = _uof.LocationRepository.Create(location);
            _uof.Commit();

            return CreatedAtAction(
                nameof(GetById),
                new { id = newLocation.Id },
                newLocation);
        }

        [HttpPut("{id}")]
        public ActionResult<Location> Update(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            var updatedLocation = _uof.LocationRepository.Update(location);
            _uof.Commit();

            if (updatedLocation == null)
            {
                return NotFound(new
                {
                    Message = $"Location with ID {id} was not found."
                });
            }

            return Ok(updatedLocation);
        }

        [HttpDelete("{id}")]
        public ActionResult<Location> Delete(int id)
        {
            var location = _uof.LocationRepository.GetById(l => l.Id == id);

            if (location == null)
            {
                return NotFound(new
                {
                    Message = $"Location with ID {id} was not found."
                });
            }

            _uof.LocationRepository.Delete(location);
            _uof.Commit();

            return Ok(location);
        }

        [HttpGet("{id:int:min(1)}/characters")]
        public ActionResult<IEnumerable<Character>> GetCharactersByLocation(int id)
        {
            var characters = _uof.LocationRepository.GetCharactersByLocation(id);

            if (!characters.Any())
            {
                return NotFound(new
                {
                    Message = $"No characters were found for location {id}."
                });
            }

            return Ok(characters);
        }
    }
}