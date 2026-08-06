using AutoMapper;
using BigBangAPI.DTO;
using BigBangAPI.Models;
using BigBangAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<LocationDTO>>> Get()
        {
            var locations = await _uof.LocationRepository
                .GetAll()
                .ToListAsync();

            var locationsDto = _mapper.Map<IEnumerable<LocationDTO>>(locations);

            return Ok(locationsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDTO>> GetById(int id)
        {
            var location = await _uof.LocationRepository.GetByIdAsync(l => l.Id == id);

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

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult<LocationDTO>> Create([FromBody] LocationCreateDTO dto)
        {
            if (dto is null)
            {
                return BadRequest();
            }

            var location = _mapper.Map<Location>(dto);

            var newLocation = _uof.LocationRepository.Create(location);

            await _uof.CommitAsync();

            var locationDto = _mapper.Map<LocationDTO>(newLocation);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newLocation.Id },
                locationDto);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult<LocationDTO>> Update(
            int id,
            [FromBody] LocationUpdateDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var location = await _uof.LocationRepository.GetByIdAsync(l => l.Id == id);

            if (location == null)
            {
                return NotFound(new
                {
                    Message = $"Location with ID {id} was not found."
                });
            }

            _mapper.Map(dto, location);

            _uof.LocationRepository.Update(location);

            await _uof.CommitAsync();

            var locationDto = _mapper.Map<LocationDTO>(location);

            return Ok(locationDto);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<LocationDTO>> Delete(int id)
        {
            var location = await _uof.LocationRepository.GetByIdAsync(l => l.Id == id);

            if (location == null)
            {
                return NotFound(new
                {
                    Message = $"Location with ID {id} was not found."
                });
            }

            _uof.LocationRepository.Delete(location);

            await _uof.CommitAsync();

            var dto = _mapper.Map<LocationDTO>(location);

            return Ok(dto);
        }

        [HttpGet("{id:int:min(1)}/characters")]
        public async Task<ActionResult<IEnumerable<CharacterDTO>>> GetCharactersByLocation(int id)
        {
            var characters = await _uof.LocationRepository.GetCharactersByLocationAsync(id);

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
