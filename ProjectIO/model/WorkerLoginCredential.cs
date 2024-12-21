
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("WorkerLoginCredentials")]
    public class WorkerLoginCredential
    {
        [Key]
        public int loginId { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [MaxLength(50)]
        public string password { get; set; }

        [ForeignKey("workerId")]
        public Worker worker { get; set; }

    }

}

