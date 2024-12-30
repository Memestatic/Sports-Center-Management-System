using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProjectIO.DataValidation
{
    public class ValidatePassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;
            if (password == null)
            {
                return new ValidationResult("Password is required.");
            }

            // Walidacja: minimum 7 znaków, jedna duża litera, jedna cyfra
            if (!Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{7,}$"))
            {
                return new ValidationResult("Password must be at least 7 characters long, contain at least one uppercase letter, and one digit.");
            }

            return ValidationResult.Success;
        }
    }
}
