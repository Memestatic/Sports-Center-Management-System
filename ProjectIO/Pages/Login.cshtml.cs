using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProjectIO.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SportCenterContext _context;

        public LoginModel(SportCenterContext context)
        {
            _context = context;
        }

        // Model do bindowania danych z formularza
        [BindProperty]
        public LoginInputModel Input { get; set; }

        // Komunikat o b³êdzie
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Wyczyszczenie komunikatu o b³êdzie przy za³adowaniu strony
            ErrorMessage = string.Empty;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Jeœli dane s¹ niepoprawne, wróæ na stronê
            }

            // Wyszukiwanie u¿ytkownika w bazie danych
            var user = _context.Users
                .FirstOrDefault(u => u.email == Input.email && u.password == Input.password);

            if (user == null)
            {
                // Jeœli u¿ytkownik nie zosta³ znaleziony
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            CurrentPerson.SetInstance(user); // Ustawienie zalogowanego u¿ytkownika

            return RedirectToPage("/Booking");
        }
    }

}
