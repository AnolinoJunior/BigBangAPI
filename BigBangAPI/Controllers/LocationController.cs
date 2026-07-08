using BigBangAPI.Models;
using BigBangAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BigBangAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _repository;

        public LocationController(ILocationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Location>> Get()
        {
            var locations = _repository.GetLocations();

            return Ok(locations);
        }

        [HttpGet("{id}")]
        public ActionResult<Location> GetById(int id)
        {
            var location = _repository.GetLocation(id);

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
            var newLocation = _repository.Create(location);

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

            var updatedLocation = _repository.Update(location);

            if (updatedLocation == null)
            {
                return NotFound();
            }

            return Ok(updatedLocation);
        }

        [HttpDelete("{id}")]
        public ActionResult<Location> Delete(int id)
        {
            var location = _repository.Delete(id);

            if (location == null)
            {
                return NotFound(new
                {
                    Message = $"Location with ID {id} was not found."
                });
            }

            return Ok(location);
        }
    }
}