using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nome de usário é obrigatório")]
        public  string? UserName { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]

        public string? Password { get; set; }
    }
}
