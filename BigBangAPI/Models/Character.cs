using BigBangAPI.Validations;
using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o nome.")]
        [StringLength(45, ErrorMessage = "O nome deve ter no máximo 45 caracteres.")]
        [PrimeiraLetraMaiuscula]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Informe o nome do ator.")]
        [StringLength(45, ErrorMessage = "O nome do ator deve ter no máximo 45 caracteres.")]
        public string Actor { get; set; } = string.Empty;
        [Required(ErrorMessage = "Informe a ocupação.")]
        [StringLength(45,MinimumLength = 10, 
            ErrorMessage = "A ocupação deve ter entre 10 e 45 caracteres.")]
        public string Occupation { get; set; } = string.Empty;
        [Required(ErrorMessage = "Informe a universidade.")]
        [StringLength(45, ErrorMessage = "O nome da universidade deve ter no máximo 45 caracteres.")]
        public string University { get; set; } = string.Empty;
        public int IQ { get; set; }

        public int Age { get; set; }
        public string? ImageUrl { get; set; }
        public int LocationId { get; set; }
        
        public Location ? Location { get; set; }

        public ICollection<CharacterEpisode>? CharacterEpisodes { get; set; }
    }
}
