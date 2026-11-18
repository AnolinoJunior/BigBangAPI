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

        /// <summary>Lista as localizações.</summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDTO>>> Get()
        {
            var locations = await _uof.LocationRepository
                .GetAll()
                .ToListAsync();

            var locationsDto = _mapper.Map<IEnumerable<LocationDTO>>(locations);

            return Ok(locationsDto);
        }

        /// <summary>Consulta uma localização pelo ID.</summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDTO>> GetById(int id)
        {
            var location = await _uof.LocationRepository.GetByIdAsync(l => l.Id == id);

            if (location == null)
            {
                return NotFound(new
                {
                    Message = $"A localização com ID {id} não foi encontrada."
                });
            }

            var dto = _mapper.Map<LocationDTO>(location);

            return Ok(dto);
        }

        /// <summary>Cadastra uma localização.</summary>
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

        /// <summary>Atualiza uma localização.</summary>
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
                    Message = $"A localização com ID {id} não foi encontrada."
                });
            }

            _mapper.Map(dto, location);

            _uof.LocationRepository.Update(location);

            await _uof.CommitAsync();

            var locationDto = _mapper.Map<LocationDTO>(location);

            return Ok(locationDto);
        }

        /// <summary>Exclui uma localização.</summary>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<LocationDTO>> Delete(int id)
        {
            var location = await _uof.LocationRepository.GetByIdAsync(l => l.Id == id);

            if (location == null)
            {
                return NotFound(new
                {
                    Message = $"A localização com ID {id} não foi encontrada."
                });
            }

            _uof.LocationRepository.Delete(location);

            await _uof.CommitAsync();

            var dto = _mapper.Map<LocationDTO>(location);

            return Ok(dto);
        }

        /// <summary>Lista os personagens vinculados à localização.</summary>
        [HttpGet("{id:int:min(1)}/characters")]
        public async Task<ActionResult<IEnumerable<CharacterDTO>>> GetCharactersByLocation(int id)
        {
            var characters = await _uof.LocationRepository.GetCharactersByLocationAsync(id);

            if (!characters.Any())
            {
                return NotFound(new
                {
                    Message = $"Nenhum personagem encontrado para a localização {id}."
                });
            }

            var charactersDto = _mapper.Map<IEnumerable<CharacterDTO>>(characters);

            return Ok(charactersDto);
        }
    }
}
