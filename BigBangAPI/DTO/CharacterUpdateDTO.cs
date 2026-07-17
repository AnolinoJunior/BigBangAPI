using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class CharacterUpdateDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Actor { get; set; } = string.Empty;

    [Required]
    public string Occupation { get; set; } = string.Empty;

    public string University { get; set; } = string.Empty;

    public int IQ { get; set; }

    public int Age { get; set; }

    public int LocationId { get; set; }

    public string? ImageUrl { get; set; }
}