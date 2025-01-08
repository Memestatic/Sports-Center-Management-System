using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;

namespace ProjectIO.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SportCenterContext _context;

        public RegisterModel(SportCenterContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; }

        public IActionResult OnGet()
        {
            int? id = HttpContext.Session.GetInt32("userID");
            
            if (id != null)
            {
                return RedirectToPage("/Account/ClientPanel");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Jeśli model jest niepoprawny, wróć do formularza
            }

            User u = _context.Users.FirstOrDefault(u => u.Email.Equals(Input.Email));

            if (u != null)
            {
                return RedirectToPage("/Account/KnownEmail"); 
            }

            // Tworzymy instancję PasswordHasher
            var passwordHasher = new PasswordHasher<User>();

            User user = new User
            {
                Name = Input.Name,
                Surname = Input.Surname,
                DeclaredGender = Input.DeclaredGender,
                PhoneNumber = Input.PhoneNumber,
                Email = Input.Email,
                Password = Input.Password
            };

            // Hashowanie hasła
            user.Password = passwordHasher.HashPassword(user, user.Password);

            // Dodanie użytkownika do bazy danych z zahashowanym hasłem
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login"); // Przekierowanie po sukcesie
        }
    }

}