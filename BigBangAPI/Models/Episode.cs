using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.Models
{
    public class Episode
    {
            [Key]
            public int Id { get; set; }
            [StringLength(45, ErrorMessage = "Title cannot be longer than 45 characters.")]
            public string Title { get; set; } = string.Empty;
            [Range(1, 12, ErrorMessage = "Season must be between 1 and 12.")]
            public int Season { get; set; }
            public int EpisodeNumber { get; set; }
            public DateTime AirDate { get; set; }
            public ICollection<CharacterEpisode>? CharacterEpisodes { get; set; }
        }
    }

