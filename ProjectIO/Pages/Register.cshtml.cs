using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly SportCenterContext _context;

        public RegisterModel(SportCenterContext context)
        {
            _context = context;
        }

        // Bindowanie danych bezpoœrednio do encji User
        [BindProperty]
        public User Input { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Jeœli model jest niepoprawny, wróæ do formularza
            }

            // Dodanie u¿ytkownika do bazy danych
            _context.Users.Add(Input);
            await _context.SaveChangesAsync();

            return RedirectToPage("/"); // Przekierowanie po sukcesie
        }

    }
}