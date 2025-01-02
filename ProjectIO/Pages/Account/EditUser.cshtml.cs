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

        public void OnGet()
        {
            if(CurrentPerson.GetInstance().Equals(null))
            {
                Response.Redirect("/Login");
            }
            User user = (User)CurrentPerson.GetInstance();

            Input = new UpdateInputModel
            {
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                DeclaredGender = user.DeclaredGender
            };

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User user = (User)CurrentPerson.GetInstance();
            user.Name = Input.Name;
            user.Surname = Input.Surname;
            user.PhoneNumber = Input.PhoneNumber;
            user.DeclaredGender = Input.DeclaredGender;

            _context.Attach(user).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToPage("/Account/ClientPanel");
        }
    }
}
