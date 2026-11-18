using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class CharacterUpdateDTO
{
    [Required(ErrorMessage = "Informe o ID.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(50, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o nome do ator.")]
    public string Actor { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a ocupação.")]
    public string Occupation { get; set; } = string.Empty;

    public string University { get; set; } = string.Empty;

    public int IQ { get; set; }

    public int Age { get; set; }

    public int LocationId { get; set; }

    public string? ImageUrl { get; set; }
}