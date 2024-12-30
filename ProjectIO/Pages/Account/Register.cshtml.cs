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
            if (CurrentPerson.GetInstance() != null)
            {
                // Jeśli użytkownik jest już zalogowany, przekieruj go do panelu klienta
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

            User u = _context.Users.FirstOrDefault(u => u.email.Equals(Input.email));

            if (u != null)
            {
                return RedirectToPage("/Account/KnownEmail" new {name = Input.name, surname = Input.surname, gender = Input.gender.ToString(), phone = Input.phone, email = Input.email}); 
            }

            // Tworzymy instancję PasswordHasher
            var passwordHasher = new PasswordHasher<User>();

            User user = new User
            {
                name = Input.name,
                surname = Input.surname,
                gender = Input.gender,
                phone = Input.phone,
                email = Input.email,
                password = Input.password
            };

            // Hashowanie hasła
            user.password = passwordHasher.HashPassword(user, user.password);

            // Dodanie użytkownika do bazy danych z zahashowanym hasłem
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login"); // Przekierowanie po sukcesie
        }
    }

}