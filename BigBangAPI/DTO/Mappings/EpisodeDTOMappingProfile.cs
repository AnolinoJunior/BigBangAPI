using AutoMapper;
using BigBangAPI.Models;

namespace BigBangAPI.DTO.Mappings
{
    public class EpisodeDTOMappingProfile : Profile
    {
        public EpisodeDTOMappingProfile()
        {
            CreateMap<Episode, EpisodeDTO>().ReverseMap();

            CreateMap<EpisodeCreateDTO, Episode>();

            CreateMap<EpisodeUpdateDTO, Episode>();
        }
    }
}