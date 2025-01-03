using ProjectIO.DataValidation;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
namespace ProjectIO.WebModels
{
    public class UpdateInputModel
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


    }
}
