
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int userId { get; set; }

        [Required]
        [MaxLength(50)]
        public string name { get; set; }

        [Required]
        [MaxLength(50)]
        public string surname { get; set; }

        [Required]
        public Gender gender { get; set; }

        [Required]
        [Phone]
        public string phone { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [MaxLength(50)]
        public string password { get; set; }


    }
}


