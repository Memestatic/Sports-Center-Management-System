using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProjectIO.Pages.Account
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
                .FirstOrDefault(u => u.email == Input.email);

            if (user == null)
            {
                // Je�li u�ytkownik nie zosta� znaleziony
                ModelState.AddModelError("Input.email", "The email address is not registered.");
                return Page();
            }

            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user, user.password, Input.password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Input.password", "The password is incorrect.");
                return Page();
            }

            CurrentPerson.SetInstance(user); // Ustawienie zalogowanego u�ytkownika

            return RedirectToPage("/Account/ClientPanel");
        }
    }

}
