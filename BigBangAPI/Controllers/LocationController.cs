using AutoMapper;
using BigBangAPI.DTO;
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
        private readonly IMapper _mapper;

        public LocationController(
            IUnityOfWork uof,
            IMapper mapper)
        {
            _uof = uof;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<LocationDTO>> Get()
        {
            var locations = _uof.LocationRepository.GetAll();

            var locationsDto = _mapper.Map<IEnumerable<LocationDTO>>(locations);

            return Ok(locationsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<LocationDTO> GetById(int id)
        {
            var location = _uof.LocationRepository.GetById(l => l.Id == id);

            if (location == null)
            {
                return NotFound(new
                {
                    Message = $"Location with ID {id} was not found."
                });
            }

            var dto = _mapper.Map<LocationDTO>(location);

            return Ok(dto);
        }

        [HttpPost]
        public ActionResult<LocationDTO> Create([FromBody] LocationCreateDTO dto)
        {
            if (dto is null)
            {
                return BadRequest();
            }

            var location = _mapper.Map<Location>(dto);

            var newLocation = _uof.LocationRepository.Create(location);

            _uof.Commit();

            var locationDto = _mapper.Map<LocationDTO>(newLocation);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newLocation.Id },
                locationDto);
        }

        [HttpPut("{id}")]
        public ActionResult<LocationDTO> Update(int id, [FromBody] LocationUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var location = _mapper.Map<Location>(dto);

            var updatedLocation = _uof.LocationRepository.Update(location);

            if (updatedLocation == null)
            {
                return NotFound(new
                {
                    Message = $"Location with ID {id} was not found."
                });
            }

            _uof.Commit();

            var locationDto = _mapper.Map<LocationDTO>(updatedLocation);

            return Ok(locationDto);
        }

        [HttpDelete("{id}")]
        public ActionResult<LocationDTO> Delete(int id)
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

            var dto = _mapper.Map<LocationDTO>(location);

            return Ok(dto);
        }

        [HttpGet("{id:int:min(1)}/characters")]
        public ActionResult<IEnumerable<CharacterDTO>> GetCharactersByLocation(int id)
        {
            var characters = _uof.LocationRepository.GetCharactersByLocation(id);

            if (!characters.Any())
            {
                return NotFound(new
                {
                    Message = $"No characters were found for location {id}."
                });
            }

            var charactersDto = _mapper.Map<IEnumerable<CharacterDTO>>(characters);

            return Ok(charactersDto);
        }
    }
}