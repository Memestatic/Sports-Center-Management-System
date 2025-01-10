
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public int ReservationId { get; set; }

        [ForeignKey("FacilityId")]
        public required Facility ReservationFacility { get; set; }

        [ForeignKey("UserId")]
        public required User ReservationUser { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public Status CurrentStatus { get; set; }
        
        [Required]
        public bool IsChangingRoomReserved { get; set; }
        
        [Required]
        public bool IsEquipmentReserved { get; set; }

        [Required]
        public double Cost { get; set; }

    }
}

