using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO;

public class LocationUpdateDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(45)]
    public string Name { get; set; } = string.Empty;
}