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

        public string MinDate { get; set; }
        public string MaxDate { get; set; }


        public BookingModel(SportCenterContext context)
        {
            _context = context;
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

            if(CurrentPerson.GetInstance() != null)
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

                    HttpContext.Session.Remove("selectedObjectId");
                    HttpContext.Session.SetInt32("selectedObjectId", (int) SelectedObjectId);
                    
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
                            reservation.CurrentReservationStatus == ReservationStatus.Approved)
                        {
                            string slot = $"{reservation.ReservationFacility.FacilitySportsCenter.SportsCenterId} {reservation.ReservationFacility.FacilityId} {reservation.ReservationDate:yyyy-MM-dd HH}";
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

            if (!DateTime.TryParseExact(
                $"{SelectedDay} {SelectedHour}:00:00",
                "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var reservationDate))
            {
                return BadRequest("Invalid date or time format.");
            }
            if (reservationDate < DateTime.Now)
            {
                return BadRequest("Cannot book a past time.");
            }

            SportsCenters = SportsCenters = _context.SportsCenters.ToList() ?? new List<SportsCenter>();
            Facilities = _context.Facilities.ToList();

            // Upewnij si�, �e Facilities nie jest null
            if (Facilities == null || !Facilities.Any())
            {
                return BadRequest("Facilities list is not available or empty.");
            }

            

            int? selectedObjectId = HttpContext.Session.GetInt32("selectedObjectId");

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

            _context.Users.Attach(currentPerson);

            var reservation = new Reservation
            {
                ReservationFacility = facility,
                ReservationUser = currentPerson,
                ReservationDate = reservationDate,
                CurrentReservationStatus = ReservationStatus.Pending
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToPage("/Payment", new { reservationId = reservation.ReservationId });
        }

    }
}
