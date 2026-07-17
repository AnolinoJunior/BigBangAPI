using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class EpisodeCreateDTO
{
    [Required]
    [StringLength(45)]
    public string Title { get; set; } = string.Empty;

    [Range(1, 12)]
    public int Season { get; set; }

    public int EpisodeNumber { get; set; }

    public DateTime AirDate { get; set; }
}