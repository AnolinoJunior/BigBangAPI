using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "Informe o nome de usuário.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Informe o e-mail.")]

        public string? Email { get; set; }

        [Required(ErrorMessage = "Informe a senha.")]

        public string? Password { get; set; }

    }
}
