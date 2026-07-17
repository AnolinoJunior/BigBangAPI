using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class LocationCreateDTO
{
    [Required]
    [StringLength(45)]
    public string Name { get; set; } = string.Empty;
}