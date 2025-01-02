using System.ComponentModel.DataAnnotations;
using ProjectIO.DataValidation;

namespace ProjectIO.model
{
    public abstract class Person
    {
        [Required]
        [MaxLength(50)]
        public string name { get; set; }

        [Required]
        [MaxLength(50)]
        public string surname { get; set; }

        [Required]
        public Gender gender { get; set; }

        [Required]
        [ValidatePhone]
        public string phone { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [MaxLength(255)]
        [ValidatePassword]
        public string password { get; set; }
    }
}
