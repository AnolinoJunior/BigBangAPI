using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.Models
{
    public class Episode
    {
            [Key]
            public int Id { get; set; }
            [StringLength(45, ErrorMessage = "O título deve ter no máximo 45 caracteres.")]
            public string Title { get; set; } = string.Empty;
            [Range(1, 12, ErrorMessage = "A temporada deve estar entre 1 e 12.")]
            public int Season { get; set; }
            public int EpisodeNumber { get; set; }
            public DateTime AirDate { get; set; }
            public ICollection<CharacterEpisode>? CharacterEpisodes { get; set; }
        }
    }

