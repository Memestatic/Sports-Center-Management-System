using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;

namespace ProjectIO.Pages.Account
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public List<VerificationToken> VerificationTokens { get; set; }

        private readonly SportCenterContext _context;
        private readonly IConfiguration _configuration;

        public RegisterModel(SportCenterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            this.VerificationTokens = _context.VerificationTokens.ToList();

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
                Password = Input.Password,
                IsActive = false
            };

            // Hashowanie hasła
            user.Password = passwordHasher.HashPassword(user, user.Password);

            // Dodanie użytkownika do bazy danych z zahashowanym hasłem
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            string token = Guid.NewGuid().ToString();

            // Zapis tokena w bazie
            VerificationToken verificationToken = new VerificationToken
            {
                VerifiedUser = user,
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddHours(24) // Token ważny 24 godziny
            };

            _context.VerificationTokens.Add(verificationToken);
            await _context.SaveChangesAsync();

            // Wysyłanie e-maila weryfikacyjnego
            var verificationUrl = Url.Page(
                "/Account/Verify",
                null,
                new { token = token },
                Request.Scheme);

            var marketing = new Marketing(_context, _configuration);
            await marketing.SendVerificationEmail(user.Name, user.Email, verificationUrl);


            return RedirectToPage("/Account/Login"); // Przekierowanie po sukcesie
        }

        
    }

}