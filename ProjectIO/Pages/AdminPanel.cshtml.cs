using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectIO.model;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Reflection;
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
        public List<WorkerFunction> WorkerFunctions { get; set; }

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

            if (CurrentPerson.GetInstance() is User)
            {
                return BadRequest("Login into your worker account first");
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
            WorkerFunctions = _context.WorkerFunctions.ToList();

            if (!string.IsNullOrEmpty(tab))
            {
                ActiveTab = tab;
            }


            // Obs³uga ró¿nych zak³adek
            switch (tab)
            {
                case "tab1":
                    SportsCenters = _context.SportsCenters.ToList(); // Przyk³ad dla zak³adki 1
                    break;
                case "tab2":
                    Facilities = _context.Facilities.ToList(); // Przyk³ad dla zak³adki 2
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
                // Dodaj wiêcej przypadków dla pozosta³ych zak³adek
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

        //SPORTSCENTER////////////////////////////////////////////////////////////////////////////
        public IActionResult OnPostDelete(int id)
        {
            var sc = _context.SportsCenters.Find(id);
            if (sc != null)
            {
                _context.SportsCenters.Remove(sc);
                _context.SaveChanges();
            }
            return RedirectToPage(); // Przekierowanie po usuniêciu
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

        //FACILITY/SPORTSOBJECT/////////////////////////////////////////////////////////////
        public IActionResult OnPostAddF(string facilityName, string centerName, string typeName, bool isChangingRoomAvailable, bool isEquipmentAvailable, DateTime promoStart, DateTime promoEnd, double promoRate)
        {
            // Znalezienie SportsCenter na podstawie nazwy
            var sportsCenter = _context.SportsCenters.FirstOrDefault(sc => sc.centerName == centerName);
            if (sportsCenter == null)
            {
                ModelState.AddModelError(string.Empty, "Specified Sports Center not found.");
                return RedirectToPage();
            }

            // Znalezienie FacilityType na podstawie typu (typeName)
            var facilityType = _context.FacilityTypes.FirstOrDefault(ft => ft.typeName == typeName);
            if (facilityType == null)
            {
                ModelState.AddModelError(string.Empty, "Specified Facility Type not found.");
                return RedirectToPage();
            }

            // Walidacja zakresu dat promocji
            if (promoEnd <= promoStart)
            {
                ModelState.AddModelError(string.Empty, "Promotion end date must be later than start date.");
                return RedirectToPage();
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
                _context.Facilities.Remove(facility); // Usuniêcie obiektu z kontekstu
                _context.SaveChanges(); // Zapisanie zmian w bazie danych
            }
            return RedirectToPage(); // Przekierowanie po usuniêciu
        }

        public IActionResult OnPostEditF(int id, string facilityName, string centerName, string typeName, bool isChangingRoomAvailable, bool isEquipmentAvailable, DateTime promoStart, DateTime promoEnd, double promoRate)
        {
            var fac = _context.Facilities
                              .Include(f => f.sportsCenter)
                              .Include(f => f.facilityType)
                              .FirstOrDefault(f => f.facilityId == id);

            if (fac != null)
            {
                fac.facilityName = facilityName;
                fac.sportsCenter.centerName = centerName;
                fac.facilityType.typeName = typeName;
                fac.isChangingRoomAvailable = isChangingRoomAvailable;
                fac.isEquipmentAvailable = isEquipmentAvailable;
                fac.promoStart = promoStart;
                fac.promoEnd = promoEnd;
                fac.promoRate = promoRate;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        //USERS/////////////////////////////////////////////////////////////////////////////
        public IActionResult OnPostDeleteUser(int id)
        {
            var usr = _context.Users.Find(id);
            if (usr != null)
            {
                _context.Users.Remove(usr);
                _context.SaveChanges();
            }
            return RedirectToPage(); // Przekierowanie po usuniêciu
        }

        public IActionResult OnPostEditUser(int id, string userName, string userSurname, Gender userGender, string userPhone, string userEmail, string userPassword)
        {
            var usr = _context.Users.Find(id);
            if (usr != null)
            {
                usr.name = userName;
                usr.surname = userSurname;
                usr.gender = userGender;
                usr.phone = userPhone;
                usr.email = userEmail;
                usr.password = userPassword;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostAddUser(string userName, string userSurname, Gender userGender, string userPhone, string userEmail, string userPassword)
        {
            var usr = new User
            {
                name = userName,
                surname = userSurname,
                gender = userGender,
                phone = userPhone,
                email = userEmail,
                password = userPassword
            };

            _context.Users.Add(usr);
            _context.SaveChanges();
            return RedirectToPage();
        }

        //WORKERS//////////////////////////////////////////////////
        public IActionResult OnPostDeleteWorker(int id)
        {
            var wrk = _context.Workers.Find(id);
            if (wrk != null)
            {
                _context.Workers.Remove(wrk);
                _context.SaveChanges();
            }
            return RedirectToPage(); // Przekierowanie po usuniêciu
        }

        public IActionResult OnPostEditWorker(int id, int workerFunction, string workerName, string workerSurname, Gender workerGender, string workerPhone, string workerEmail, string workerPassword)
        {
            var functionId = _context.WorkerFunctions.FirstOrDefault(fc => fc.functionId == workerFunction);
            var wrk = _context.Workers.Find(id);
            if (wrk != null)
            {
                wrk.function = functionId;
                wrk.name = workerName;
                wrk.surname = workerSurname;
                wrk.gender = workerGender;
                wrk.phone = workerPhone;
                wrk.email = workerEmail;
                wrk.password = workerPassword;

                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostAddWorker(int workerFunction, string workerName, string workerSurname, Gender workerGender, string workerPhone, string workerEmail, string workerPassword)
        {

            var functionId = _context.WorkerFunctions.FirstOrDefault(fc => fc.functionId == workerFunction);

            var wrk = new Worker
            {
                function = functionId,
                name = workerName,
                surname = workerSurname,
                gender = workerGender,
                phone = workerPhone,
                email = workerEmail,
                password = workerPassword
            };

            _context.Workers.Add(wrk);
            _context.SaveChanges();
            return RedirectToPage();
        }

    }
}
