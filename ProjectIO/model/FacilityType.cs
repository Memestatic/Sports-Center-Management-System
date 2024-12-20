namespace ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("FacilityTypes")]
public class FacilityType
{
    [Key]
    public int typeId { get; set; }

    [Required]
    [MaxLength(50)]
    public string typeName { get; set; }
}
