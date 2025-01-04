
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Passes")]
    public class Pass
    {
        [Key]
        public int PassId { get; set; }
        
        [Required]
        [ForeignKey("PassTypeId")]
        public required PassType PassType { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public required User PassUser { get; set; }

        [Required]
        public int PassEntriesLeft { get; set; }
        
        [Required]
        public Status CurrentStatus { get; set; }

    }
}



