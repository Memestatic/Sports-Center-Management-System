namespace ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("PassTypes")]
public class PassType
{
    [Key]
    public int passTypeId { get; set; }

    [Required]
    [MaxLength(50)]
    public string passTypeName { get; set; }

    [Required]
    public int passTypeDuration { get; set; }
}
