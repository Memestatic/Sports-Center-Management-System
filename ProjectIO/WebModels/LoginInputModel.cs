using System.ComponentModel.DataAnnotations;

namespace ProjectIO.model
{
    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
    }
}
