
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("WorkerTrainingSessions")]
    public class WorkerTrainingSession
    {
        public int workerId { get; set; }
        public int sessionId { get; set; }

        //TODO: dodac klucz glowny
        //wymaga konfiguracji klucza glownego z dwoch kluczy obcych
    }

}

