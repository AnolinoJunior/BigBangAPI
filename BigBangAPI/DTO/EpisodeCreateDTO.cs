using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class EpisodeCreateDTO
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    [StringLength(45, ErrorMessage = "O título não pode ter mais de 45 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [Range(1, 12, ErrorMessage = "A temporada deve estar entre 1 e 12.")]
    public int Season { get; set; }

    public int EpisodeNumber { get; set; }

    public DateTime AirDate { get; set; }
}
