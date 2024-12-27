using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Serialization;
using ProjectIO.model;
using Microsoft.EntityFrameworkCore;


namespace ProjectIO.Pages
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
                return RedirectToPage("/Login");
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
            return RedirectToPage("/Payment", new { reservationId = reservationId });
        }
    }
}
