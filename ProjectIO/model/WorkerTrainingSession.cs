
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("WorkerTrainingSessions")]
    public class WorkerTrainingSession
    {
        [Key]
        public int workerTrainingSessionId { get; set; }
        public int workerId { get; set; }
        public int sessionId { get; set; }

        //TODO: dodac klucz glowny
        //wymaga konfiguracji klucza glownego z dwoch kluczy obcych
    }

}

