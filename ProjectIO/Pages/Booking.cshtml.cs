using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using ProjectIO.model;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace ProjectIO.Pages
{
    public class BookingModel : PageModel
    {
        public List<SportsCenter> SportsCenters { get; set; }
        public List<Facility> Facilities { get; set; }
        public int? SelectedCenterId { get; set; }
        public string? SelectedCenterName { get; set; }
        [BindProperty]
        public int? SelectedObjectId { get; set; }

        public Facility selectedObject { get; set; }

        public string? SelectedObjectName { get; set; }

        public List<Reservation> Reservations { get; set; }
        public List<TrainingSession> TrainingSessions { get; set; }

        public List<string> TakenSlots { get; set; } = new List<string>();

        [BindProperty]
        public string SelectedDay { get; set; }

        [BindProperty]
        public int SelectedHour { get; set; }

        [BindProperty]
        public bool isUserLogged { get; set; }


        private readonly SportCenterContext _context;
        private readonly IConfiguration _configuration;

        public string MinDate { get; set; }
        public string MaxDate { get; set; }
        
        [BindProperty]
        public bool IsLockroom { get; set; }
        
        [BindProperty]
        public bool IsGear { get; set; }


        public BookingModel(SportCenterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            IsLockroom = false;
            IsGear = false;
        }


        public void OnGet(int? centerId, int? objectId, string? selectedDay)
        {
            // Minimalna data to dzi�
            MinDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Maksymalna data - np. rezerwacje tylko do 3 miesi�cy naprz�d
            MaxDate = DateTime.Now.AddMonths(3).ToString("yyyy-MM-dd");

            SportsCenters = SportsCenters = _context.SportsCenters.ToList() ?? new List<SportsCenter>();
            Facilities = _context.Facilities.ToList();
            Reservations = _context.Reservations.ToList();
            TrainingSessions = _context.TrainingSessions.ToList();

            if (CurrentPerson.GetInstance() != null)
            {
                isUserLogged = true;
            }
            else {
                isUserLogged = false;
            }

            SelectedDay = selectedDay ?? DateTime.Now.ToString("yyyy-MM-dd");

            // Obs�uga wybranego o�rodka
            if (centerId.HasValue)
            {
                var selectedCenter = SportsCenters.FirstOrDefault(c => c.SportsCenterId == centerId.Value);
                if (selectedCenter != null)
                {
                    SelectedCenterId = selectedCenter.SportsCenterId;
                    SelectedCenterName = selectedCenter.Name;

                    // Filtrowanie obiekt�w przypisanych do wybranego o�rodka
                    Facilities = Facilities.Where(f => f.FacilitySportsCenter.SportsCenterId == centerId.Value).ToList();
                }
            }

            // Obs�uga wybranego obiektu
            if (objectId.HasValue && Facilities.Any())
            {
                selectedObject = Facilities.FirstOrDefault(f => f.FacilityId == objectId.Value);
                if (selectedObject != null)
                {
                    SelectedObjectId = selectedObject.FacilityId;
                    SelectedObjectName = selectedObject.FacilityName;

                    HttpContext.Session.Remove("SelectedObjectId");
                    HttpContext.Session.SetInt32("SelectedObjectId", (int) SelectedObjectId);
                    
                }
            }

            if (!string.IsNullOrEmpty(SelectedDay))
                Console.WriteLine(SelectedDay);
            else
                Console.WriteLine("Nie ustawiono");

            // Obs�uga wybranego dnia
            if (!string.IsNullOrEmpty(SelectedDay) && SelectedObjectId.HasValue)
            {
                DateTime selectedDate;
                if (DateTime.TryParse(SelectedDay, out selectedDate))
                {
                    foreach (var reservation in Reservations)
                    {
                        if (reservation.ReservationFacility.FacilitySportsCenter.SportsCenterId == SelectedCenterId &&
                            reservation.ReservationFacility.FacilityId == SelectedObjectId &&
                            reservation.ReservationDate.Date == selectedDate.Date &&
                            reservation.CurrentStatus == Status.Approved)
                        {
                            string slot = $"{reservation.ReservationFacility.FacilitySportsCenter.SportsCenterId} {reservation.ReservationFacility.FacilityId} {reservation.ReservationDate:yyyy-MM-dd HH}";
                            if (!TakenSlots.Contains(slot))
                            {
                                TakenSlots.Add(slot);
                            }
                        }
                    }

                    foreach (var session in TrainingSessions)
                    {
                        if (session.Facility.FacilitySportsCenter.SportsCenterId == SelectedCenterId &&
                            session.Facility.FacilityId == SelectedObjectId &&
                            session.Date.Date == selectedDate.Date)
                        {
                            string slot = $"{session.Facility.FacilitySportsCenter.SportsCenterId} {session.Facility.FacilityId} {session.Date:yyyy-MM-dd HH}";
                            if (!TakenSlots.Contains(slot))
                            {
                                TakenSlots.Add(slot);
                            }
                        }
                    }

                    // Dodaj przesz�e godziny dla bie��cego dnia jako "zaj�te"
                    if (selectedDate.Date == DateTime.Now.Date)
                    {
                        for (int pastHour = 0; pastHour < DateTime.Now.Hour; pastHour++)
                        {
                            string pastSlot = $"{SelectedCenterId} {SelectedObjectId} {selectedDate:yyyy-MM-dd} {pastHour:00}";
                            if (!TakenSlots.Contains(pastSlot))
                            {
                                TakenSlots.Add(pastSlot);
                            }
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            if (CurrentPerson.GetInstance() is Worker)
            {
                return BadRequest("Login into your customer account first");
            }

            if (!DateTime.TryParse(
            $"{SelectedDay} {SelectedHour}:00",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var reservationDate))
            {
                return BadRequest("Invalid date or time format.");
            }


            SportsCenters = SportsCenters = _context.SportsCenters.ToList() ?? new List<SportsCenter>();
            Facilities = _context.Facilities.ToList();

            // Upewnij si�, �e Facilities nie jest null
            if (Facilities == null || !Facilities.Any())
            {
                return BadRequest("Facilities list is not available or empty.");
            }

            

            int? selectedObjectId = HttpContext.Session.GetInt32("SelectedObjectId");

            var facility = Facilities.FirstOrDefault(f => f.FacilityId == selectedObjectId);
            if (facility == null)
            {
                return BadRequest("Facility not found.");
            }

            var currentPerson = (User)CurrentPerson.GetInstance();
            if (currentPerson is not User user)
            {
                return Unauthorized();
            }

            //_context.Users.Attach(currentPerson);
            
            User client = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);

            var reservation = new Reservation
            {
                ReservationFacility = facility,
                ReservationUser = client,
                ReservationDate = reservationDate,
                CurrentStatus = Status.Pending,
                IsChangingRoomReserved = IsLockroom,
                IsEquipmentReserved = IsGear
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            // Send confirmation email
            try
            {
                SendConfirmationEmail(reservation);
            }
            catch (Exception ex)
            {
                // Log the error and inform the user
                Console.WriteLine($"Error sending email: {ex.Message}");
            }

            return RedirectToPage("/Account/ChoosePaymentMethod", new { OrderId = "r" + reservation.ReservationId });
        }

        private void SendConfirmationEmail(Reservation reservation)
        {
            var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("EVERSPORT", emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(reservation.ReservationUser.Name, reservation.ReservationUser.Email));
            email.Subject = "Reservation Confirmation";
            email.Body = new TextPart("plain")
            {
                Text = $"Dear {reservation.ReservationUser.Name},\n\n" +
                       $"Your reservation has been successfully created.\n\n" +
                       $"Reservation Details:\n" +
                       $"- Facility: {reservation.ReservationFacility.FacilityName}\n" +
                       $"- Date: {reservation.ReservationDate:yyyy-MM-dd}\n" +
                       $"- Time: {reservation.ReservationDate:HH:mm}\n" +
                       $"- Changing Room Reserved: {(reservation.IsChangingRoomReserved ? "Yes" : "No")}\n" +
                       $"- Equipment Reserved: {(reservation.IsEquipmentReserved ? "Yes" : "No")}\n\n" +
                       $"Thank you for choosing us!\n\nBest regards,\nEversport Team"
            };

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings.SmtpServer, emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings.SenderEmail, emailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

    }
}
