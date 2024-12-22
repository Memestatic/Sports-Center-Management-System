
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("TrainingSessions")]
    public class TrainingSession
    {
        [Key]
        public int sessionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string sessionName { get; set; }

        [Required]
        public DateTime sessionDate { get; set; }

        [Required]
        public int sessionDuration { get; set; }

        [Required]
        public int sessionCapacity { get; set; }


    }

}

