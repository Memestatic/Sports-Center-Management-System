using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (CurrentPerson.GetInstance() == null)
            {
                return RedirectToPage("/Index"); // Strona g³ówna
            }

            // Usuniêcie instancji u¿ytkownika
            CurrentPerson.EndInstance();

            // Przekierowanie na stronê g³ówn¹ lub logowania
            return RedirectToPage("/Index"); // Strona g³ówna
        }
    }
}
