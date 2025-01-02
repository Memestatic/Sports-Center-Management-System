
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("SportsCenters")]
    public class SportsCenter
    {
        [Key]
        public int SportsCenterId { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Street { get; set; }

        [Required]
        [MaxLength(5)]
        public required string StreetNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public required string City { get; set; }

        [Required]
        [MaxLength(50)]
        public required string State { get; set; }

        [Required]
        [MaxLength(50)]
        public required string ZipCode { get; set; }




    }
}


