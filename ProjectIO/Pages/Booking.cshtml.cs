using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages
{
    public class BookingModel : PageModel
    {
        public List<SportsCenter> SportsCenters { get; set; } = new();
        public List<Facility> Facilities { get; set; } = new();
        public int? SelectedCenterId { get; set; }
        public string? SelectedCenterName { get; set; }
        public int? SelectedObjectId { get; set; }
        public string? SelectedObjectName { get; set; }

        public void OnGet(int? centerId, int? objectId)
        {
            // Tymczasowe dane o�rodk�w
            SportsCenters = new List<SportsCenter>
        {
            new SportsCenter { centerId = 1, centerName = "Center 1" },
            new SportsCenter { centerId = 2, centerName = "Center 2" }
        };

            // Tymczasowe dane obiekt�w z powi�zaniami
            Facilities = new List<Facility>
        {
            new Facility { facilityId = 1, facilityName = "Facility 1", sportsCenter = SportsCenters[0] },
            new Facility { facilityId = 2, facilityName = "Facility 2", sportsCenter = SportsCenters[1] }
        };

            // Obs�uga wybranego o�rodka
            if (centerId.HasValue)
            {
                var selectedCenter = SportsCenters.FirstOrDefault(c => c.centerId == centerId.Value);
                if (selectedCenter != null)
                {
                    SelectedCenterId = selectedCenter.centerId;
                    SelectedCenterName = selectedCenter.centerName;

                    // Filtrowanie obiekt�w przypisanych do wybranego o�rodka
                    Facilities = Facilities.Where(f => f.sportsCenter.centerId == centerId.Value).ToList();
                }
            }

            // Obs�uga wybranego obiektu
            if (objectId.HasValue && Facilities.Any())
            {
                var selectedObject = Facilities.FirstOrDefault(f => f.facilityId == objectId.Value);
                if (selectedObject != null)
                {
                    SelectedObjectId = selectedObject.facilityId;
                    SelectedObjectName = selectedObject.facilityName;
                }
            }
        }
    }
}
