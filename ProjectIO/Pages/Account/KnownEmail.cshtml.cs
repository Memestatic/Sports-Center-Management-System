using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages.Account
{
    public class KnownEmailModel : PageModel
    {
        public IActionResult OnGet()
        {
            int? id = HttpContext.Session.GetInt32("userID");
            
            if (id != null)
            {
                return RedirectToPage("/Account/ClientPanel");
            }
            
            return Page();
        }


    }
}
