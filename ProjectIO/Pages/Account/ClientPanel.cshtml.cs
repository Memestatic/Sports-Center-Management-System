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
        [BindProperty]
        public User CurrentUser { get; set; }

        [BindProperty]
        public bool MarketingConsent { get; set; }

        public IActionResult OnGet()
        {
            int? id = HttpContext.Session.GetInt32("userID");
            
            if (id == null)
            {
                return RedirectToPage("/Account/Login");
            }
            
            CurrentUser = _context.Users.FirstOrDefault(u => u.UserId == id);
            
            MarketingConsent = CurrentUser.MarketingConsent; // Pobierz aktualn� warto��
            
            
            return Page();
        }

        public IActionResult OnPost()
        {
            int? id = HttpContext.Session.GetInt32("userID");
            
            if (id == null)
            {
                return RedirectToPage("/Account/Login");
            }
            
            var currentUser = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (currentUser is Worker)
            {
                return BadRequest("Login into your customer account first");
            }
            
            currentUser.MarketingConsent = MarketingConsent; // Aktualizacja zgody
            _context.SaveChanges(); // Zapisz zmiany w bazie danych

            return RedirectToPage("/Account/ClientPanel");
        }
    }
}
