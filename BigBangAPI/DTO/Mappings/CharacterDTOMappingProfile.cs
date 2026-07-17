using AutoMapper;
using BigBangAPI.Models;

namespace BigBangAPI.DTO.Mappings
{
    public class CharacterDTOMappingProfile : Profile
    {
        public CharacterDTOMappingProfile()
        {
            // Entity -> DTO (Lista)
            CreateMap<Character, CharacterDTO>()
                .ReverseMap();

            // Entity -> DTO (Detalhes)
            CreateMap<Character, CharacterDetailsDTO>()
                .ForMember(dest => dest.LocationName,
                    opt => opt.MapFrom(src =>
                        src.Location != null ? src.Location.Name : null))
                .ForMember(dest => dest.Episodes,
                    opt => opt.MapFrom(src =>
                        src.CharacterEpisodes != null
                            ? src.CharacterEpisodes
                                .Select(ce => ce.Episode != null
                                    ? ce.Episode.Title
                                    : string.Empty)
                                .ToList()
                            : new List<string>()));

            // DTO de criação -> Entity
            CreateMap<CharacterCreateDTO, Character>();

            // DTO de atualização -> Entity
            CreateMap<CharacterUpdateDTO, Character>();
        }
    }
}