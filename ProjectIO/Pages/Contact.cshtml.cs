using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProjectIO.model;


namespace ProjectIO.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public string StatusMessage { get; set; }

        private readonly IConfiguration _configuration;

        public ContactModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Invalid input.";
                return Page();
            }

            // Pobierz ustawienia z appsettings.json
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Contact Form", emailSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("Admin", emailSettings.SenderEmail));
            emailMessage.Subject = "New message from contact form";
            emailMessage.Body = new TextPart("plain")
            {
                Text = $"You have received a new message from {Email}:\n\n{Message}"
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(emailSettings.SmtpServer, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(emailSettings.SenderEmail, emailSettings.Password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }

                StatusMessage = "Message sent successfully!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error sending message: {ex.Message}";
            }

            return Page();
        }

    }
}
