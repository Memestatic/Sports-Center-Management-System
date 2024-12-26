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

        // Komunikat o b��dzie
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Wyczyszczenie komunikatu o b��dzie przy za�adowaniu strony
            ErrorMessage = string.Empty;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Je�li dane s� niepoprawne, wr�� na stron�
            }

            // Wyszukiwanie u�ytkownika w bazie danych
            var user = _context.Users
                .FirstOrDefault(u => u.email == Input.email && u.password == Input.password);

            if (user == null)
            {
                // Je�li u�ytkownik nie zosta� znaleziony
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            CurrentPerson.SetInstance(user); // Ustawienie zalogowanego u�ytkownika

            return RedirectToPage("/Booking");
        }
    }

}
