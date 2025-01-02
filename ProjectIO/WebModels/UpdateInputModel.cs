using ProjectIO.DataValidation;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
namespace ProjectIO.WebModels
{
    public class UpdateInputModel
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


    }
}
