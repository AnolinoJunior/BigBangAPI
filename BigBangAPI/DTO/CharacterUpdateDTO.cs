using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class CharacterUpdateDTO
{
    [Required(ErrorMessage = "O identificador é obrigatório.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome não pode ter mais de 50 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome do ator é obrigatório.")]
    public string Actor { get; set; } = string.Empty;

    [Required(ErrorMessage = "A ocupação é obrigatória.")]
    public string Occupation { get; set; } = string.Empty;

    public string University { get; set; } = string.Empty;

    public int IQ { get; set; }

    public int Age { get; set; }

    public int LocationId { get; set; }

    public string? ImageUrl { get; set; }
}
