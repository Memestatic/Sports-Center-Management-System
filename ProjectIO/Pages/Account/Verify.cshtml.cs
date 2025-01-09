using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
using System;

namespace ProjectIO.Pages.Account
{
    public class VerifyModel : PageModel
    {
        [BindProperty]
        public List<User> Users { get; set; }

        [BindProperty]
        public List<VerificationToken> VerificationTokens { get; set; }

        private readonly SportCenterContext _context;

        public VerifyModel(SportCenterContext context)
        {
            _context = context;
        }

        public string Message { get; private set; }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            this.Users = _context.Users.ToList();
            this.VerificationTokens = _context.VerificationTokens.ToList();

            if (string.IsNullOrEmpty(token))
            {
                Message = "Invalid or missing token.";
                return Page();
            }

            // ZnajdŸ token w bazie danych
            var verificationToken = VerificationTokens
                .FirstOrDefault(t => t.Token == token);

            if (verificationToken == null || verificationToken.ExpirationDate < DateTime.UtcNow)
            {
                Message = "Token is invalid or has expired.";
                return Page();
            }

            // Aktywuj u¿ytkownika
            var user = Users.FirstOrDefault(u => u.UserId == verificationToken.VerifiedUser.UserId);
            if (user == null)
            {
                Message = "User not found.";
                return Page();
            }

            user.IsActive = true;

            // Usuñ token z bazy danych (opcjonalnie)
            _context.VerificationTokens.Remove(verificationToken);

            await _context.SaveChangesAsync();

            Message = "Your account has been successfully verified!";
            return Page();
        }
    }
}
