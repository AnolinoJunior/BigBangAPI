using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Informe o nome de usuário.")]
        public  string? UserName { get; set; }

        [Required(ErrorMessage = "Informe a senha.")]

        public string? Password { get; set; }
    }
}
