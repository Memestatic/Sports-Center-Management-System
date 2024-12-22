
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Workers")]
    public class Worker
    {
        [Key]
        public int workerId { get; set; }

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

        [ForeignKey("functionId")]
        public WorkerFunction function { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [MaxLength(50)]
        public string password { get; set; }



    }

}

