using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("VerificationTokens")]
    public class VerificationToken
    {
        [Key]
        public int TokenId { get; set; }
        [ForeignKey("UserId")]
        public required User VerifiedUser { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }

    }
}
