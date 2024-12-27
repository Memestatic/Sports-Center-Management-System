using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIO.model;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata;

namespace ProjectIO.Pages
{
    public class AdminPanel : PageModel
    {
        private readonly SportCenterContext _context;
        public int Permissions { get; set; }
        public AdminPanel(SportCenterContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {

            if (CurrentPerson.GetInstance() == null)
            {
                return RedirectToPage("/WorkLogin");
            }

            var perms = CurrentPerson.GetInstance() as Worker;

            Permissions = _context.Workers
               .Where(w => w.workerId == perms.workerId)
               .Select(w => w.function.functionId)
               .FirstOrDefault();


            Console.WriteLine(Permissions);

            return Page();
            //}
        }
    }
}
