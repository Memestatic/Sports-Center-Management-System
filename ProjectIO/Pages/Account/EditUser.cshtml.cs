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
                name = user.name,
                surname = user.surname,
                phone = user.phone,
                gender = user.gender
            };

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User user = (User)CurrentPerson.GetInstance();
            user.name = Input.name;
            user.surname = Input.surname;
            user.phone = Input.phone;
            user.gender = Input.gender;

            _context.Attach(user).State = EntityState.Modified;
            _context.SaveChanges();

            return RedirectToPage("/Account/ClientPanel");
        }
    }
}
