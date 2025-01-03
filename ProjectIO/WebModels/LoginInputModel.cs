using System.ComponentModel.DataAnnotations;

namespace ProjectIO.model
{
    public class LoginInputModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
