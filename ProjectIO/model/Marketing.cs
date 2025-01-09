using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net;

namespace ProjectIO.model
{
    public class Marketing
    {
        public List<User> Users { get; set; }

        private readonly IConfiguration _configuration;
        private readonly SportCenterContext _context;

        public Marketing(SportCenterContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task SendMarketingEmails(Facility facility)
        {
            // Pobierz użytkowników ze zgodą marketingową
            var usersWithConsent = _context.Users.Where(u => u.MarketingConsent).ToList();

            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            foreach (var user in usersWithConsent)
            {
                try
                {
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("EVERSPORT", emailSettings.SenderEmail));
                    email.To.Add(new MailboxAddress(user.Name, user.Email));
                    email.Subject = "Exclusive Discount Just for You!";

                    // Calculate discounted price
                    var discountedPrice = facility.Price - (facility.Price * facility.PromoRate / 100);

                    email.Body = new TextPart("plain")
                    {
                        Text = $"Dear {user.Name},\n\n" +
                               $"We are thrilled to inform you about our exclusive discount at {facility.FacilityName}!\n\n" +
                               $"Here are the details:\n" +
                               $"- Original Price: {facility.Price.ToString("C", new System.Globalization.CultureInfo("pl-PL"))}\n" +
                               $"- Discount: {facility.PromoRate}%\n" +
                               $"- New Price: {discountedPrice.ToString("C", new System.Globalization.CultureInfo("pl-PL"))}\n\n" +
                               $"Don't miss this opportunity to enjoy our top-notch facilities at a reduced price.\n\n" +
                               $"Thank you for choosing EVERSPORT!\n\n" +
                               $"Best regards,\n" +
                               $"The Eversport Team"
                    };

                    using var smtp = new SmtpClient();
                    smtp.Connect(emailSettings.SmtpServer, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate(emailSettings.SenderEmail, emailSettings.Password);
                    smtp.Send(email);
                    smtp.Disconnect(true);

                    Console.WriteLine($"Email sent successfully to {user.Email}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email to {user.Email}: {ex.Message}");
                }
            }
        }

        public async Task SendVerificationEmail(string userName, string email, string verificationUrl)
        {
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("EVERSPORT", emailSettings.SenderEmail));
                emailMessage.To.Add(new MailboxAddress(userName, email));
                emailMessage.Subject = "Confirm Your Registration";

                emailMessage.Body = new TextPart("plain")
                {
                    Text = $"Dear User,\n\n" +
                           $"Thank you for registering at EVERSPORT!\n\n" +
                           $"Please confirm your account by clicking the link below:\n" +
                           $"{verificationUrl}\n\n" +
                           $"Once your account is confirmed, you can start enjoying our premium sports facilities and exclusive offers.\n\n" +
                           $"If you have any questions, feel free to contact our support team.\n\n" +
                           $"Thank you for choosing EVERSPORT!\n\n" +
                           $"Best regards,\n" +
                           $"The EVERSPORT Team"
                };

                using var smtp = new SmtpClient();
                smtp.Connect(emailSettings.SmtpServer, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(emailSettings.SenderEmail, emailSettings.Password);
                await smtp.SendAsync(emailMessage);
                smtp.Disconnect(true);

                Console.WriteLine($"Verification email sent successfully to {email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send verification email to {email}: {ex.Message}");
            }
        }

    }
}

