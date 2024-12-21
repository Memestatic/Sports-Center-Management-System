
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ProjectIO.model
{
    [Table("WorkerFunctions")]
    public class WorkerFunction
    {
        [Key]
        public int functionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string functionName { get; set; }

    }

}
