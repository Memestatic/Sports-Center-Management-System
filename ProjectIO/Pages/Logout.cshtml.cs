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
                return RedirectToPage("/Index"); // Strona g��wna
            }

            // Usuni�cie instancji u�ytkownika
            CurrentPerson.EndInstance();

            // Przekierowanie na stron� g��wn� lub logowania
            return RedirectToPage("/Index"); // Strona g��wna
        }
    }
}
