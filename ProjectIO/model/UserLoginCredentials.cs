
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("UserLoginCredentials")]
    public class UserLoginCredential
    {
        [Key]
        public int loginId { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [MaxLength(50)]
        public string password { get; set; }

        [ForeignKey("userId")]
        public User user { get; set; }

    }

}

