using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata;

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

        public List<string> TakenSlots { get; set; } = new List<string>();

        [BindProperty]
        public string SelectedDay { get; set; }

        [BindProperty]
        public int SelectedHour { get; set; }

        [BindProperty]
        public bool isUserLogged { get; set; }


        private readonly SportCenterContext _context;

        public BookingModel(SportCenterContext context)
        {
            _context = context;
        }


        public void OnGet(int? centerId, int? objectId, string? selectedDay)
        {
            SportsCenters = SportsCenters = _context.SportsCenters.ToList() ?? new List<SportsCenter>();
            Facilities = _context.Facilities.ToList();
            Reservations = _context.Reservations.ToList();

            if(CurrentPerson.GetInstance() != null)
            {
                isUserLogged = true;
            }
            else {
                isUserLogged = false;
            }

            SelectedDay = selectedDay ?? DateTime.Now.ToString("yyyy-MM-dd");

            // Obs³uga wybranego oœrodka
            if (centerId.HasValue)
            {
                var selectedCenter = SportsCenters.FirstOrDefault(c => c.centerId == centerId.Value);
                if (selectedCenter != null)
                {
                    SelectedCenterId = selectedCenter.centerId;
                    SelectedCenterName = selectedCenter.centerName;

                    // Filtrowanie obiektów przypisanych do wybranego oœrodka
                    Facilities = Facilities.Where(f => f.sportsCenter.centerId == centerId.Value).ToList();
                }
            }

            // Obs³uga wybranego obiektu
            if (objectId.HasValue && Facilities.Any())
            {
                selectedObject = Facilities.FirstOrDefault(f => f.facilityId == objectId.Value);
                if (selectedObject != null)
                {
                    SelectedObjectId = selectedObject.facilityId;
                    SelectedObjectName = selectedObject.facilityName;

                    HttpContext.Session.Remove("selectedObjectId");
                    HttpContext.Session.SetInt32("selectedObjectId", (int) SelectedObjectId);
                    
                }
            }

            if (!string.IsNullOrEmpty(SelectedDay))
                Console.WriteLine(SelectedDay);
            else
                Console.WriteLine("Nie ustawiono");

            // Obs³uga wybranego dnia
            if (!string.IsNullOrEmpty(SelectedDay) && SelectedObjectId.HasValue)
            {
                DateTime selectedDate;
                if (DateTime.TryParse(SelectedDay, out selectedDate))
                {
                    foreach (var reservation in Reservations)
                    {
                        if (reservation.facility.sportsCenter.centerId == SelectedCenterId &&
                            reservation.facility.facilityId == SelectedObjectId &&
                            reservation.reservationDate.Date == selectedDate.Date &&
                            reservation.reservationStatus != ReservationStatus.Denied)
                        {
                            string slot = $"{reservation.facility.sportsCenter.centerId} {reservation.facility.facilityId} {reservation.reservationDate:yyyy-MM-dd HH}";
                            if (!TakenSlots.Contains(slot))
                            {
                                TakenSlots.Add(slot);
                            }
                        }
                    }

                    var exampleReservation = new Reservation
                    {
                        reservationId = 1, // Jeœli bazy danych u¿ywa klucza autoinkrementuj¹cego, ten numer zostanie nadpisany.
                        facility = Facilities.FirstOrDefault(f => f.facilityId == 1, null),
                        user = (User)CurrentPerson.GetInstance(),
                        reservationDate = new DateTime(2024, 12, 26, 10, 0, 0),
                        reservationStatus = ReservationStatus.Pending // Ustawienie statusu na "Oczekuj¹cy"
                    };

                    if (exampleReservation.facility != null)
                    {
                        string slot1 = $"{exampleReservation.facility.sportsCenter.centerId} {exampleReservation.facility.facilityId} {exampleReservation.reservationDate:yyyy-MM-dd HH}";
                        Console.WriteLine($"Slot1: {slot1}");
                        TakenSlots.Add(slot1);
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            SportsCenters = SportsCenters = _context.SportsCenters.ToList() ?? new List<SportsCenter>();
            Facilities = _context.Facilities.ToList();

            // Upewnij siê, ¿e Facilities nie jest null
            if (Facilities == null || !Facilities.Any())
            {
                return BadRequest("Facilities list is not available or empty.");
            }

            if (!DateTime.TryParseExact(
                $"{SelectedDay} {SelectedHour}:00:00",
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var reservationDate))
            {
                return BadRequest("Invalid date or time format.");
            }

            int? selectedObjectId = HttpContext.Session.GetInt32("selectedObjectId");

            var facility = Facilities.FirstOrDefault(f => f.facilityId == selectedObjectId);
            if (facility == null)
            {
                return BadRequest("Facility not found.");
            }

            var currentPerson = (User)CurrentPerson.GetInstance();
            if (currentPerson is not User user)
            {
                return Unauthorized();
            }

            _context.Users.Attach(currentPerson);

            var reservation = new Reservation
            {
                facility = facility,
                user = currentPerson,
                reservationDate = reservationDate,
                reservationStatus = ReservationStatus.Pending
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToPage("/Payment");
        }

    }
}
