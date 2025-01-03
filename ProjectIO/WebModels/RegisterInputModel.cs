using ProjectIO.DataValidation;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;

public class RegisterInputModel: Person
{

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public required string VerifyPassword { get; set; }
}
