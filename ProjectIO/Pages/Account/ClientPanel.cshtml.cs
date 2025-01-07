using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages.Account
{
    public class ClientPanelModel : PageModel
    {
        private readonly SportCenterContext _context; // Kontekst bazy danych
        public ClientPanelModel(SportCenterContext context)
        {
            _context = context;
        }
        public User CurrentUser { get; set; }

        [BindProperty]
        public bool MarketingConsent { get; set; }

        public IActionResult OnGet()
        {
            var currentUser = CurrentPerson.GetInstance();
            if (currentUser == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (currentUser is Worker)
            {
                return BadRequest("Login into your customer account first");
            }

            CurrentUser = currentUser as User;

            // Pobierz warto�� zgody marketingowej z bazy danych
            var userFromDb = _context.Users.FirstOrDefault(u => u.UserId == CurrentUser.UserId);
            if (userFromDb != null)
            {
                MarketingConsent = userFromDb.MarketingConsent; // Pobierz aktualn� warto��
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var currentUser = CurrentPerson.GetInstance();
            if (currentUser == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (currentUser is Worker)
            {
                return BadRequest("Login into your customer account first");
            }

            CurrentUser = currentUser as User;

            // Aktualizuj zgod� marketingow�
            var userFromDb = _context.Users.FirstOrDefault(u => u.UserId == CurrentUser.UserId);
            if (userFromDb != null)
            {
                userFromDb.MarketingConsent = MarketingConsent; // Aktualizacja zgody
                _context.SaveChanges(); // Zapisz zmiany w bazie danych
            }

            return RedirectToPage("/Account/ClientPanel");
        }
    }
}
