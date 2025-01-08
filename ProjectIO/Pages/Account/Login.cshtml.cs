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

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return RedirectToPage("/Account/ClientPanel");
            }

            // Wyczyszczenie komunikatu o b��dzie przy za�adowaniu strony
            ErrorMessage = string.Empty;

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Je�li dane s� niepoprawne, wr�� na stron�
            }

            // Wyszukiwanie u�ytkownika w bazie danych
            var user = _context.Users
                .FirstOrDefault(u => u.Email == Input.Email);

            if (user == null)
            {
                // Je�li u�ytkownik nie zosta� znaleziony
                ModelState.AddModelError("Input.Email", "The Email address is not registered.");
                return Page();
            }

            var passwordHasher = new PasswordHasher<User>();

            var result = passwordHasher.VerifyHashedPassword(user, user.Password, Input.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Input.Password", "The Password is incorrect.");
                return Page();
            }
            
            HttpContext.Session.SetInt32("userID", user.UserId);
            /*if (HttpContext.Session.GetInt32("workerID") != null)
            {
                HttpContext.Session.Remove("workerID");
            }*/
            return RedirectToPage("/Account/ClientPanel");
        }
    }

}
