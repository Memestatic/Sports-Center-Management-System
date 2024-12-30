using ProjectIO.DataValidation;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;

public class RegisterInputModel: Person
{

    [Required]
    [Compare("password", ErrorMessage = "Passwords do not match.")]
    public string Verifypassword { get; set; }
}
