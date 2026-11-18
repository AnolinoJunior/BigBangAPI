using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class LocationCreateDTO
{
    [Required(ErrorMessage = "Informe o nome.")]
    [StringLength(45, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
    public string Name { get; set; } = string.Empty;
}