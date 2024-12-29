using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages.Account
{
    public class ClientPanelModel : PageModel
    {

        public IActionResult OnGet()
        {
            var currentUser = CurrentPerson.GetInstance();
            if (currentUser == null)
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }
    }
}
