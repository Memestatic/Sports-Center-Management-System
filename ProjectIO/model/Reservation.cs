
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectIO.model
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public int reservationId { get; set; }

        [ForeignKey("facilityId")]
        public Facility facility { get; set; }

        [ForeignKey("userId")]
        public User user { get; set; }

        [Required]
        public DateTime reservationDate { get; set; }

        [Required]
        public ReservationStatus reservationStatus { get; set; }


    }
}


