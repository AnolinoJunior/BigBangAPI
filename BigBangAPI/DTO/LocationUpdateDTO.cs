using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class LocationUpdateDTO
{
    [Required(ErrorMessage = "O identificador é obrigatório.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(45, ErrorMessage = "O nome não pode ter mais de 45 caracteres.")]
    public string Name { get; set; } = string.Empty;
}
