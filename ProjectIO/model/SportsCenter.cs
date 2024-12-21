
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("SportsCenters")]
    public class SportsCenter
    {
        [Key]
        public int centerId { get; set; }

        [Required]
        [MaxLength(50)]
        public string centerName { get; set; }

        [Required]
        [MaxLength(50)]
        public string centerStreet { get; set; }

        [Required]
        [MaxLength(5)]
        public string centerStreetNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string centerCity { get; set; }

        [Required]
        [MaxLength(50)]
        public string centerState { get; set; }

        [Required]
        [MaxLength(50)]
        public string centerZip { get; set; }




    }
}


