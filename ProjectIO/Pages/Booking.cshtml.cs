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


        private readonly SportCenterContext _context;

        public BookingModel(SportCenterContext context)
        {
            _context = context;
        }

        public void OnGet(int? centerId, int? objectId)
        {

            SportsCenters = SportsCenters = _context.SportsCenters.ToList() ?? new List<SportsCenter>();
            Facilities = _context.Facilities.ToList();
            Reservations = _context.Reservations.ToList();

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
                    foreach (var reservation in Reservations)
                    {
                        if (reservation.facility.sportsCenter.centerId == SelectedCenterId &&
                            reservation.facility.facilityId == SelectedObjectId &&
                            reservation.reservationStatus != ReservationStatus.Denied)
                        {
                            string slot = $"{reservation.reservationDate:yyyy-MM-dd HH}";
                            TakenSlots.Add(slot);
                        }
                    }

                }
            }


        }

        public void OnPost()
        {
            Console.WriteLine("SelectedDay: " + SelectedDay);

        }
    }
}
