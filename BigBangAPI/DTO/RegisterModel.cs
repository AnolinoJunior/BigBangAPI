using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Nome de usário é obrigatório")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email é  Obrigatório")]

        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]

        public string? Password { get; set; }

    }
}
