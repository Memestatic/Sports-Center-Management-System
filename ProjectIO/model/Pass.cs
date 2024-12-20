namespace ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Passes")]
public class Pass
{
    [Key]
    public int passId { get; set; }

    [ForeignKey("passTypeId")]
    public int passTypeId { get; set; }

    [Required]
    public int passEntriesLeft { get; set; }
}
}
