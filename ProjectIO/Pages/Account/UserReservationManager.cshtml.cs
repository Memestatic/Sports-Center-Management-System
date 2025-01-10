using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Serialization;
using ProjectIO.model;
using Microsoft.EntityFrameworkCore;


namespace ProjectIO.Pages.Account
{
    public class UserReservationManagerModel : PageModel
    {
        [BindProperty]
        public User currentUser { get; set; }

        [BindProperty]
        public List<Reservation> reservations { get; set; }

        private readonly SportCenterContext _context;

        public UserReservationManagerModel(SportCenterContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            int? id = HttpContext.Session.GetInt32("userID");
            
            if (id == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (HttpContext.Session.GetInt32("workerID") != null)
            {
                return BadRequest("Login into your customer account first");
            }

            currentUser = _context.Users.FirstOrDefault(u => u.UserId == id);

            reservations = _context.Reservations
                .Include(r => r.ReservationFacility) // �aduje obiekt ReservationFacility
                .ThenInclude(f => f.FacilitySportsCenter) // �aduje obiekt FacilitySportsCenter wewn�trz ReservationFacility
                .Include(r => r.ReservationUser) // �aduje obiekt PassUser
                .Where(r => r.ReservationUser.UserId == currentUser.UserId)
                .ToList();

            Console.WriteLine(reservations);

            return Page();
        }

        public IActionResult OnPostPayHandler(int reservationId)
        {
            return RedirectToPage("/Account/ChoosePaymentMethod", new { orderId = "r" + reservationId });
        }

        public IActionResult OnPostDenyHandler(int reservationId)
        {
            // Znajd� rezerwacj� o podanym ID w bazie danych
            var reservationToDeny = _context.Reservations.FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservationToDeny == null)
            {
                // Je�li rezerwacja nie istnieje, mo�esz wy�wietli� b��d lub przekierowa�
                ModelState.AddModelError("", "Reservation not found.");
                return Page();
            }

            // Usu� rezerwacj� z bazy danych
            reservationToDeny.CurrentStatus = Status.Denied;

            // Zapisz zmiany w bazie danych
            _context.SaveChanges();

            // Prze�aduj stron�, aby od�wie�y� list� rezerwacji
            return RedirectToPage();
        }
    }
}
