using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages.Account
{
    public class KnownEmailModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (CurrentPerson.GetInstance() != null)
            {
                // Jeœli u¿ytkownik jest ju¿ zalogowany, przekieruj go do panelu klienta
                return RedirectToPage("/Account/ClientPanel");
            }
            return Page();
        }


    }
}
