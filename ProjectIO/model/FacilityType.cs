
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("FacilityTypes")]
    public class FacilityType
    {
        [Key]
        public int TypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string TypeName { get; set; }
    }
}

