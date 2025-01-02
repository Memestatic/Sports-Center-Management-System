using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIO.model;

namespace ProjectIO.Pages
{
    public class PaymentModel : PageModel
    {
        [BindProperty]
        public int reservationId { get; set; }

        [BindProperty]
        public Reservation reservation { get; set; }

        private readonly SportCenterContext _context;

        public PaymentModel(SportCenterContext context)
        {
            _context = context;
        }
        public void OnGet(int reservationId)
        {
            //this.ReservationId. = ReservationId;
            this.reservationId = reservationId;
            reservation = _context.Reservations.FirstOrDefault(r => r.ReservationId == reservationId);


        }

        public IActionResult OnPost()
        {
            var reservation = _context.Reservations
                    .Include(r => r.ReservationFacility) 
                    .FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation == null)
            {
                return NotFound("Rezerwacja nie znaleziona");
            }

            if (reservation.CurrentReservationStatus != ReservationStatus.Pending)
            {
                return NotFound("Rezerwacja nie jest w stanie oczekuj�cym");
            }

            reservation.CurrentReservationStatus = ReservationStatus.Approved;

            List<Reservation> toCancel = _context.Reservations
                .Where(r => r.ReservationDate == reservation.ReservationDate
                && r.ReservationFacility.FacilityId == reservation.ReservationFacility.FacilityId
                && r.ReservationId != reservationId)
                .ToList();

            foreach(var r in toCancel)
            {
                r.CurrentReservationStatus = ReservationStatus.Denied;
            }

            _context.SaveChanges();

            return RedirectToPage("/Account/UserReservationManager");
        }

        
    }
}
