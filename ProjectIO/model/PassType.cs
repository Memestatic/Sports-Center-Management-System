
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
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
}

