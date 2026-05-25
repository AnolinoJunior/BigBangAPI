using System.ComponentModel.DataAnnotations;

namespace BigBangAPI.Validations
{
    public class PrimeiraLetraMaiuscula : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object? value,
            ValidationContext validationContext)
        {
            var texto = value as string;

            if (string.IsNullOrEmpty(texto))
            {
                return ValidationResult.Success;
            }

            if (char.IsUpper(texto[0]))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(
                "A primeira letra deve ser maiúscula.");
        }
    }
}