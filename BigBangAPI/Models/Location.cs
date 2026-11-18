    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    namespace BigBangAPI.Models
    {
        public class Location
        {
            [Required(ErrorMessage = "Informe o ID.")]
            [Key]
            public int Id { get; set; }
            [StringLength(45, ErrorMessage = "O nome deve ter no máximo 45 caracteres.")]
            public string?  Name { get; set; }

            [JsonIgnore]
            public List<Character>? Characters { get; set; }
        }
    }
