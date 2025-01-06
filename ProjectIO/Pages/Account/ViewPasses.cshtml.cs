using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIO.model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectIO.Pages.Account
{
    public class ViewPassesModel : PageModel
    {
        private readonly SportCenterContext _context;

        public ViewPassesModel(SportCenterContext context)
        {
            _context = context;
        }

        public List<Pass> UserPasses { get; set; } = new List<Pass>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (CurrentPerson.GetInstance() == null)
            {
                return RedirectToPage("/Account/Login");
            }
            else if (CurrentPerson.GetInstance() is Worker)
            {
                return BadRequest("Login into your client account first");
            }

            var user = CurrentPerson.GetInstance() as User;

            var userId = user.UserId;

            // Pobieranie karnetów u¿ytkownika
            UserPasses = await _context.Passes
                .Include(p => p.PassType)
                .Where(p => p.PassUser.UserId == userId)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostPayNowHandler(int passId)
        {
            return RedirectToPage("/Payment", new { OrderId = "p" + passId.ToString() });
        }

        public async Task<IActionResult> OnPostDenyHandler(int passId)
        {
            var pass = await _context.Passes.FindAsync(passId);
            if (pass != null)
            {
                // Odrzuæ karnet
                _context.Passes.Remove(pass);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
