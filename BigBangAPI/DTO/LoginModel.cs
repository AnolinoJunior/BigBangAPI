using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO
{
    public class LoginModel
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public  string? UserName { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]

        public string? Password { get; set; }
    }
}
