
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Workers")]
    public class Worker : Person
    {
        [Key]
        public int WorkerId { get; set; }

        [ForeignKey("WorkerFunctionId")]
        public required WorkerFunction AssignedWorkerFunction { get; set; }
    }

}