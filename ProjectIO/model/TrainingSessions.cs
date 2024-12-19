namespace ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TrainingSessions")]
public class TrainingSessions
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
