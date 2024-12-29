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
            if (CurrentPerson.GetInstance() == null)
            {
                return RedirectToPage("/Account/Login");
            }

            currentUser = (User)CurrentPerson.GetInstance();

            reservations = _context.Reservations
                .Include(r => r.facility) // £aduje obiekt facility
                .ThenInclude(f => f.sportsCenter) // £aduje obiekt sportsCenter wewn¹trz facility
                .Include(r => r.user) // £aduje obiekt user
                .Where(r => r.user.userId == currentUser.userId)
                .ToList();

            Console.WriteLine(reservations);

            return Page();
        }

        public IActionResult OnPostPayHandler(int reservationId)
        {
            return RedirectToPage("/Payment", new { reservationId });
        }

        public IActionResult OnPostDenyHandler(int reservationId)
        {
            // ZnajdŸ rezerwacjê o podanym ID w bazie danych
            var reservationToRemove = _context.Reservations.FirstOrDefault(r => r.reservationId == reservationId);

            if (reservationToRemove == null)
            {
                // Jeœli rezerwacja nie istnieje, mo¿esz wyœwietliæ b³¹d lub przekierowaæ
                ModelState.AddModelError("", "Reservation not found.");
                return Page();
            }

            // Usuñ rezerwacjê z bazy danych
            _context.Reservations.Remove(reservationToRemove);

            // Zapisz zmiany w bazie danych
            _context.SaveChanges();

            // Prze³aduj stronê, aby odœwie¿yæ listê rezerwacji
            return RedirectToPage();
        }
    }
}
