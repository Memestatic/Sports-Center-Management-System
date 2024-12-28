using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIO.model;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata;

namespace ProjectIO.Pages
{
    public class AdminPanel : PageModel
    {

        public List<SportsCenter> SportsCenters { get; set; }
        public List<Facility> Facilities { get; set; }
        public List<FacilityType> FacilityTypes { get; set; }
        public List<User> Users { get; set; }
        public List<Worker> Workers { get; set; }
        public List<Reservation> Reservations { get; set; }

        public string ActiveTab { get; set; }
        public int Permissions { get; set; }

        private readonly SportCenterContext _context;
        public AdminPanel(SportCenterContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string tab)
        {
            if (CurrentPerson.GetInstance() == null)
            {
                return RedirectToPage("/WorkLogin");
            }

            var perms = CurrentPerson.GetInstance() as Worker;

            Permissions = _context.Workers
               .Where(w => w.workerId == perms.workerId)
               .Select(w => w.function.functionId)
               .FirstOrDefault();


            SportsCenters = _context.SportsCenters.ToList();
            Facilities = _context.Facilities.ToList();
            FacilityTypes = _context.FacilityTypes.ToList();
            Users = _context.Users.ToList();
            Workers = _context.Workers.ToList();
            Reservations = _context.Reservations.ToList();

            if (!string.IsNullOrEmpty(tab))
            {
                ActiveTab = tab;
            }


            // Obs�uga r�nych zak�adek
            switch (tab)
            {
                case "tab1":
                    SportsCenters = _context.SportsCenters.ToList(); // Przyk�ad dla zak�adki 1
                    break;
                case "tab2":
                    Facilities = _context.Facilities.ToList(); // Przyk�ad dla zak�adki 2
                    break;
                case "tab3":
                    Users = _context.Users.ToList();
                    break;
                case "tab4":
                    Workers = _context.Workers.ToList();
                    break;
                case "tab5":

                    break;
                case "tab6":
                    Facilities = _context.Facilities.ToList();
                    break;
                case "tab7":
                    Reservations = _context.Reservations.ToList();
                    break;
                // Dodaj wi�cej przypadk�w dla pozosta�ych zak�adek
                default:
                    // SportsCenters = _context.SportsCenters.ToList(); 
                    // Facilities = _context.Facilities.ToList();
                    // Users = _context.Users.ToList();
                    // Workers = _context.Workers.ToList();
                    // Reservations = _context.Reservations.ToList();
                    break;
            }

            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var sc = _context.SportsCenters.Find(id);
            if (sc != null)
            {
                _context.SportsCenters.Remove(sc);
                _context.SaveChanges();
            }
            return RedirectToPage(); // Przekierowanie po usuni�ciu
        }

        public IActionResult OnPostEdit(int id, string centerName, string centerStreet, string centerStreetNumber, string centerCity, string centerState, string centerZip)
        {
            var sc = _context.SportsCenters.Find(id);
            if (sc != null)
            {
                sc.centerName = centerName;
                sc.centerStreet = centerStreet;
                sc.centerStreetNumber = centerStreetNumber;
                sc.centerCity = centerCity;
                sc.centerState = centerState;
                sc.centerZip = centerZip;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostAdd(string centerName, string centerStreet, string centerStreetNumber, string centerCity, string centerState, string centerZip)
        {
            var sc = new SportsCenter
            {
                centerName = centerName,
                centerStreet = centerStreet,
                centerStreetNumber = centerStreetNumber,
                centerCity = centerCity,
                centerState = centerState,
                centerZip = centerZip
            };

            _context.SportsCenters.Add(sc);
            _context.SaveChanges();
            return RedirectToPage();
        }

        public IActionResult OnPostAddF(string facilityName, string centerName, string typeName, bool isChangingRoomAvailable, bool isEquipmentAvailable, DateTime promoStart, DateTime promoEnd, double promoRate)
        {
            // Znalezienie SportsCenter na podstawie nazwy
            var sportsCenter = _context.SportsCenters.FirstOrDefault(sc => sc.centerName == centerName);
            if (sportsCenter == null)
            {
                ModelState.AddModelError(string.Empty, "Specified Sports Center not found.");
                return Page();
            }

            // Znalezienie FacilityType na podstawie typu (typeName)
            var facilityType = _context.FacilityTypes.FirstOrDefault(ft => ft.typeName == typeName);
            if (facilityType == null)
            {
                ModelState.AddModelError(string.Empty, "Specified Facility Type not found.");
                return Page();
            }

            // Walidacja zakresu dat promocji
            if (promoEnd <= promoStart)
            {
                ModelState.AddModelError(string.Empty, "Promotion end date must be later than start date.");
                return Page();
            }

            // Tworzenie nowego obiektu Facility
            var facility = new Facility
            {
                facilityName = facilityName,
                sportsCenter = sportsCenter,
                facilityType = facilityType,
                isChangingRoomAvailable = isChangingRoomAvailable,
                isEquipmentAvailable = isEquipmentAvailable,
                promoStart = promoStart,
                promoEnd = promoEnd,
                promoRate = promoRate
            };

            // Dodanie nowego Facility do bazy danych
            _context.Facilities.Add(facility);
            _context.SaveChanges();

            // Przekierowanie po udanej operacji
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteF(int id)
        {
            // Znalezienie obiektu Facility na podstawie ID
            var facility = _context.Facilities.Find(id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility); // Usuni�cie obiektu z kontekstu
                _context.SaveChanges(); // Zapisanie zmian w bazie danych
            }
            return RedirectToPage(); // Przekierowanie po usuni�ciu
        }



    }
}
