
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("TrainingSessions")]
    public class TrainingSession
    {
        [Key]
        public int TrainingSessionId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int GroupCapacity { get; set; }


    }

}

