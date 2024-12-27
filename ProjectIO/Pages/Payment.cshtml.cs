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
            //this.reservationId. = reservationId;
            this.reservationId = reservationId;
            reservation = _context.Reservations.FirstOrDefault(r => r.reservationId == reservationId);


        }

        public IActionResult OnPost()
        {
            var reservation = _context.Reservations
                    .Include(r => r.facility) 
                    .FirstOrDefault(r => r.reservationId == reservationId);

            if (reservation == null)
            {
                return NotFound("Rezerwacja nie znaleziona");
            }

            if (reservation.reservationStatus != ReservationStatus.Pending)
            {
                return NotFound("Rezerwacja nie jest w stanie oczekuj¹cym");
            }

            reservation.reservationStatus = ReservationStatus.Approved;

            List<Reservation> toCancel = _context.Reservations
                .Where(r => r.reservationDate == reservation.reservationDate
                && r.facility.facilityId == reservation.facility.facilityId
                && r.reservationId != reservationId)
                .ToList();

            foreach(var r in toCancel)
            {
                r.reservationStatus = ReservationStatus.Denied;
            }

            _context.SaveChanges();

            return RedirectToPage("/UserReservationManager");
        }

        /*[IgnoreAntiforgeryToken]
        public IActionResult OnPostUpdateStatus(int reservationId)
        {
            var reservation = _context.Reservations.FirstOrDefault(r => r.reservationId == reservationId);
            if (reservation == null)
            {
                return NotFound("Rezerwacja nie znaleziona");
            }

            if(reservation.reservationStatus != ReservationStatus.Pending)
            {
                return NotFound("Rezerwacja nie jest w stanie oczekuj¹cym");
            }

            reservation.reservationStatus = ReservationStatus.Approved;

            List <Reservation> toCancel = _context.Reservations
                .Where(r => r.reservationDate == reservation.reservationDate 
                && r.facility.facilityId == reservation.facility.facilityId)
                .ToList();

            _context.SaveChanges();

            return RedirectToPage("/UserReservationManager");
        }*/
    }
}
