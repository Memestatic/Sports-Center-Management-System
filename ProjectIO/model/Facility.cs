
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Facilities")]
    public class Facility
    {
        [Key]
        public int facilityId { get; set; }

        [ForeignKey("typeId")]
        public FacilityType facilityType { get; set; }

        [ForeignKey("centerId")]
        public SportsCenter sportsCenter { get; set; }

        [Required]
        [MaxLength(50)]
        public string facilityName { get; set; }

        [Required]
        public bool isChangingRoomAvailable { get; set; }

        [Required]
        public bool isEquipmentAvailable { get; set; }

        [Required]
        public DateTime promoStart { get; set; }

        [Required]
        public DateTime promoEnd { get; set; }

        [Required]
        public double promoRate { get; set; }



    }

}

