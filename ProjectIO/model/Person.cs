using System.ComponentModel.DataAnnotations;
using ProjectIO.DataValidation;

namespace ProjectIO.model
{
    public abstract class Person
    {
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Surname { get; set; }

        [Required]
        public Gender DeclaredGender { get; set; }

        [Required]
        [ValidatePhone]
        public required string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MaxLength(255)]
        [ValidatePassword]
        public required string Password { get; set; }
    }
}
