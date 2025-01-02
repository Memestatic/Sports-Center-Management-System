using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProjectIO.DataValidation
{
    public class ValidatePhone : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Phone number is required.");
            }

            string phone = value.ToString();

            // Definicja wzorców:
            // 1. Dziewięć cyfr (np. 123456789)
            string patternLocal = @"^\d{9}$";

            // 2. Numer kierunkowy kraju z plusem (np. +48123456789)
            string patternInternational = @"^\+\d{1,3}\d{9}$";

            if (Regex.IsMatch(phone, patternLocal) || Regex.IsMatch(phone, patternInternational))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid PhoneNumber number format. Use 9 digits or +<country_code> followed by 9 digits.");
        }
    }
}
