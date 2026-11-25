using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.DTO
{
    public class RegisterModel
    {

        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]

        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]

        public string? Password { get; set; }

    }
}
