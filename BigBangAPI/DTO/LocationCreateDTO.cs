using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class LocationCreateDTO
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(45, ErrorMessage = "O nome não pode ter mais de 45 caracteres.")]
    public string Name { get; set; } = string.Empty;
}
