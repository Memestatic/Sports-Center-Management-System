using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("WorkerTrainingSessions")]
    public class WorkerTrainingSession
    {
        [Key, Column(Order = 0)]
        public int AssignedWorkerId { get; set; }

        [Key, Column(Order = 1)]
        public int SessionId { get; set; }

        [ForeignKey("WorkerId")]
        public required Worker AssignedWorker { get; set; }

        [ForeignKey("TrainingSessionId")]
        public required TrainingSession TrainingSession { get; set; }
    }

}