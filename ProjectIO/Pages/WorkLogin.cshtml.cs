using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectIO.Pages
{
    public class WorkLoginModel : PageModel
    {
        private readonly SportCenterContext _context;

        public WorkLoginModel(SportCenterContext context)
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
            var worker = _context.Workers
                .FirstOrDefault(w => w.Email == Input.Email);

            if (worker == null)
            {
                return RedirectToPage();
            }

            var passwordHasherForVerification = new PasswordHasher<Worker>();
            var result = passwordHasherForVerification.VerifyHashedPassword(worker, worker.Password, Input.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Input.Password", "The Password is incorrect.");
                return Page();
            }

            HttpContext.Session.SetInt32("workerID", worker.WorkerId);
            if (HttpContext.Session.GetInt32("userID") != null)
            {
                HttpContext.Session.Remove("userID");
            }

            return RedirectToPage("/AdminPanel");
        }

    }

}
