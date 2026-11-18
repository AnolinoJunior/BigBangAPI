using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class EpisodeUpdateDTO
{
    [Required(ErrorMessage = "Informe o ID.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o título.")]
    [StringLength(45, ErrorMessage = "O título deve ter no máximo {1} caracteres.")]
    public string Title { get; set; } = string.Empty;

    [Range(1, 12, ErrorMessage = "A temporada deve estar entre {1} e {2}.")]
    public int Season { get; set; }

    public int EpisodeNumber { get; set; }

    public DateTime AirDate { get; set; }
}