
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("PassTypes")]
    public class PassType
    {
        [Key]
        public int PassTypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string PassTypeName { get; set; }

        [Required]
        public int PassTypeDuration { get; set; }
    }
}

