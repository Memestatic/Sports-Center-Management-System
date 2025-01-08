using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("workerID") != null)
            {
                HttpContext.Session.Remove("workerID");
                return RedirectToPage("/Index");
            }
            else if (HttpContext.Session.GetInt32("userID") != null)
            {
                HttpContext.Session.Remove("userID");
                return RedirectToPage("/Index");
            }
            else
            {
                return RedirectToPage("/Index");
            }
            
        }
    }
}
