using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
using System.Reflection.Metadata;

namespace ProjectIO.Pages
{
    public class BookingModel : PageModel
    {
        public List<SportsCenter> SportsCenters { get; set; }
        public List<Facility> Facilities { get; set; }
        public int? SelectedCenterId { get; set; }
        public string? SelectedCenterName { get; set; }
        public int? SelectedObjectId { get; set; }
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
                var selectedObject = Facilities.FirstOrDefault(f => f.facilityId == objectId.Value);
                if (selectedObject != null)
                {
                    SelectedObjectId = selectedObject.facilityId;
                    SelectedObjectName = selectedObject.facilityName;
                }
            }

            if (!string.IsNullOrEmpty(SelectedDay))
                Console.WriteLine(SelectedDay);
            else
                Console.WriteLine("Nie ustawiono");

            // Obs³uga wybranego dnia
            if (!string.IsNullOrEmpty(SelectedDay))
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
                            string slot = $"{reservation.reservationDate:yyyy-MM-dd HH}";
                            if (!TakenSlots.Contains(slot))
                            {
                                TakenSlots.Add(slot);
                            }
                        }
                    }

                    var exampleReservation = new Reservation
                    {
                        reservationId = 1, // Jeœli bazy danych u¿ywa klucza autoinkrementuj¹cego, ten numer zostanie nadpisany.
                        facility = Facilities[0],
                        user = (User)CurrentPerson.GetInstance(),
                        reservationDate = new DateTime(2024, 12, 26, 10, 0, 0),
                        reservationStatus = ReservationStatus.Pending // Ustawienie statusu na "Oczekuj¹cy"
                    };

                    string slot1 = $"{exampleReservation.reservationDate:yyyy-MM-dd HH}";
                    TakenSlots.Add(slot1);
                }
            }
        }

        public IActionResult OnPost()
        {
            var reservationDate = DateTime.Parse($"{SelectedDay} {SelectedHour}:00:00");
            var reservation = new Reservation
            {
                facility = _context.Facilities.FirstOrDefault(f => f.facilityId == SelectedObjectId),
                user = (User)CurrentPerson.GetInstance(),
                reservationDate = reservationDate,
                reservationStatus = ReservationStatus.Pending
            };

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToPage("/Booking", new { centerId = SelectedCenterId, objectId = SelectedObjectId });
        }
    }
}
