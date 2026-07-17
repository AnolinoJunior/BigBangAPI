using AutoMapper;
using BigBangAPI.Models;

namespace BigBangAPI.DTO.Mappings
{
    public class LocationDTOMappingProfile : Profile
    {
        public LocationDTOMappingProfile()
        {
            CreateMap<Location, LocationDTO>().ReverseMap();

            CreateMap<LocationCreateDTO, Location>();

            CreateMap<LocationUpdateDTO, Location>();
        }
    }
}