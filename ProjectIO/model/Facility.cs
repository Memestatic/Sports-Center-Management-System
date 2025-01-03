
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Facilities")]
    public class Facility
    {
        [Key]
        public int FacilityId { get; set; }

        [ForeignKey("TypeId")]
        public required FacilityType FacilityType { get; set; }

        [ForeignKey("SportsCenterId")]
        public required SportsCenter FacilitySportsCenter { get; set; }

        [Required]
        [MaxLength(50)]
        public required string FacilityName { get; set; }

        [Required]
        public bool IsChangingRoom { get; set; }

        [Required]
        public bool IsEquipment { get; set; }

        [Required]
        public DateTime PromoStart { get; set; }

        [Required]
        public DateTime PromoEnd { get; set; }

        [Required]
        public double PromoRate { get; set; }



    }

}

