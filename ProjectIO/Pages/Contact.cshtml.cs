using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;


namespace ProjectIO.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Message { get; set; }

        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Invalid input.";
                return Page();
            }

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Contact Form", "customersupport@kziaja.site"));
            emailMessage.To.Add(new MailboxAddress("Admin", "customersupport@kziaja.site"));
            emailMessage.Subject = "New message from contact form";
            emailMessage.Body = new TextPart("plain")
            {
                Text = $"You have received a new message from {Email}:\n\n{Message}"
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("mail.privateemail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync("customersupport@kziaja.site", "iLe7$S,!iFYQ!/,");
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
