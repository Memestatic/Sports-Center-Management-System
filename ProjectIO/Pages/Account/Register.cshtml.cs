using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SportCenterContext _context;

        public RegisterModel(SportCenterContext context)
        {
            _context = context;
        }

        // Bindowanie danych bezpośrednio do encji User
        [BindProperty]
        public User Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Jeśli model jest niepoprawny, wróć do formularza
            }

            // Tworzymy instancję PasswordHasher
            var passwordHasher = new PasswordHasher<User>();

            // Hashowanie hasła
            Input.password = passwordHasher.HashPassword(Input, Input.password);

            // Dodanie użytkownika do bazy danych z zahashowanym hasłem
            _context.Users.Add(Input);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login"); // Przekierowanie po sukcesie
        }
    }

}