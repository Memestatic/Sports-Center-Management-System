
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Workers")]
    public class Worker : Person
    {
        [Key]
        public int workerId { get; set; }

        [ForeignKey("functionId")]
        public WorkerFunction function { get; set; }
    }

}