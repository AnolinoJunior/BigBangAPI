    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    namespace BigBangAPI.Models
    {
        public class Location
        {
            [Required]
            [Key]
            public int Id { get; set; }
            [StringLength(45, ErrorMessage = "Name cannot be longer than 45 characters.")]
            public string  Name { get; set; }

            [JsonIgnore]
            public List<Character>? Characters { get; set; }
        }
    }
