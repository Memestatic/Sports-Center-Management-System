using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIO.model;
using ProjectIO.WebModels;

namespace ProjectIO.Pages.Account
{
    public class EditUserModel : PageModel
    {
        [BindProperty]
        public UpdateInputModel Input { get; set; }

        private readonly SportCenterContext _context;

        public EditUserModel(SportCenterContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            int? id = HttpContext.Session.GetInt32("userID");
            
            if (id == null)
            {
                return RedirectToPage("/Account/Login");
            }
        
            User user = _context.Users.FirstOrDefault(u => u.UserId == id);

            Input = new UpdateInputModel
            {
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                DeclaredGender = user.DeclaredGender
            };
            return Page();
        }

        public IActionResult OnPost()
        {
            

            int? id = HttpContext.Session.GetInt32("userID");
            
            if (id == null)
            {
                return RedirectToPage("/Account/Login");
            }
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
        
            User user = _context.Users.FirstOrDefault(u => u.UserId == id);
            
            user.Name = Input.Name;
            user.Surname = Input.Surname;
            user.PhoneNumber = Input.PhoneNumber;
            user.DeclaredGender = Input.DeclaredGender;
            
            _context.SaveChanges();

            return RedirectToPage("/Account/ClientPanel");
        }
    }
}
