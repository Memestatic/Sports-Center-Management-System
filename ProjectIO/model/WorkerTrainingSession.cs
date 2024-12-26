using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("WorkerTrainingSessions")]
    public class WorkerTrainingSession
    {
        [Key, Column(Order = 0)]
        public int workerId { get; set; }

        [Key, Column(Order = 1)]
        public int sessionId { get; set; }

        [ForeignKey("workerId")]
        public Worker Worker { get; set; }

        [ForeignKey("sessionId")]
        public TrainingSession TrainingSession { get; set; }
    }

}