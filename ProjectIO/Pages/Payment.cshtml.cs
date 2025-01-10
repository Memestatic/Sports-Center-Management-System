using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProjectIO.model;
using System.Data;

namespace ProjectIO.Pages
{
    public class PaymentModel : PageModel
    {
        [BindProperty]
        public int reservationId { get; set; }
        
        [BindProperty]
        public int PassId { get; set; }

        [BindProperty]
        public Reservation reservation { get; set; }
        
        [BindProperty]
        public string OrderId { get; set; }

        private readonly SportCenterContext _context;

        public PaymentModel(SportCenterContext context)
        {
            _context = context;
        }
        public void OnGet(string orderId)
        {
            OrderId = orderId;

            if (OrderId.StartsWith('r'))
            {
                var reservationId = int.Parse(orderId.Substring(1));
                this.reservation = _context.Reservations
                    .Include(r => r.ReservationFacility)
                    .FirstOrDefault(r => r.ReservationId == reservationId);
            }
        }

        public IActionResult OnPost()
        {
            if (OrderId.StartsWith('r'))
            {
                reservationId = int.Parse(OrderId.Substring(1));
                var reservation = _context.Reservations
                    .Include(r => r.ReservationFacility) 
                    .FirstOrDefault(r => r.ReservationId == reservationId);

                if (reservation == null)
                {
                    return NotFound("Rezerwacja nie znaleziona");
                }

                if (reservation.CurrentStatus != Status.Pending)
                {
                    return NotFound("Rezerwacja nie jest w stanie oczekuj�cym");
                }

                reservation.CurrentStatus = Status.Approved;

                List<Reservation> toCancel = _context.Reservations
                    .Where(r => r.ReservationDate == reservation.ReservationDate
                                && r.ReservationFacility.FacilityId == reservation.ReservationFacility.FacilityId
                                && r.ReservationId != reservationId)
                    .ToList();


                foreach(var r in toCancel)
                {
                    r.CurrentStatus = Status.Denied;
                }

                _context.SaveChanges();

                return RedirectToPage("/Account/ClientPanel");
            }
            
            PassId = int.Parse(OrderId.Substring(1));

            var Pass = _context.Passes
                .First(p => p.PassId == PassId);
            
            if (Pass == null)
            {
                return NotFound("Karnet nie znaleziony");
            }

            if (Pass.CurrentStatus != Status.Pending)
            {
                return NotFound("Karnet nie jest w stanie oczekującym");
            }
            
            Pass.CurrentStatus = Status.Approved;
            _context.SaveChanges();
            return RedirectToPage("/Account/ClientPanel");
        }

        
    }
}
