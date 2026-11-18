using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class LocationUpdateDTO
{
    [Required(ErrorMessage = "Informe o ID.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(45, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
    public string Name { get; set; } = string.Empty;
}