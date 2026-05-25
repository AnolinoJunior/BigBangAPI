using BigBangAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.Models
{
    public class Character
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(45, ErrorMessage = "Name cannot be longer than 45 characters.")]
        [PrimeiraLetraMaiuscula]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(45, ErrorMessage = "Actor name cannot be longer than 45 characters.")]
        public string Actor { get; set; } = string.Empty;
        [Required]
        [StringLength(45,MinimumLength = 10, 
            ErrorMessage = "Occupation must be between 10 and 45 characters.")]
        public string Occupation { get; set; } = string.Empty;
        [Required]
        [StringLength(45, ErrorMessage = "University cannot be longer than 45 characters.")]
        public string University { get; set; } = string.Empty;
        public int IQ { get; set; }

        public int Age { get; set; }

        public int LocationId { get; set; }
        
        public Location ? Location { get; set; }

        public ICollection<CharacterEpisode>? CharacterEpisodes { get; set; }
    }
}
