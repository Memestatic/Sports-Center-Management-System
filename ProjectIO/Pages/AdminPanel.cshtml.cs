using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectIO.model;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
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
               .Where(w => w.WorkerId == perms.WorkerId)
               .Select(w => w.AssignedWorkerFunction.WorkerFunctionId)
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

        //SPORTSCENTER////////////////////////////////////////////////////////////////////////////
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

        public IActionResult OnPostEdit(int id, string Name, string Street, string StreetNumber, string City, string State, string ZipCode)
        {
            var sc = _context.SportsCenters.Find(id);
            if (sc != null)
            {
                sc.Name = Name;
                sc.Street = Street;
                sc.StreetNumber = StreetNumber;
                sc.City = City;
                sc.State = State;
                sc.ZipCode = ZipCode;
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostAdd(string Name, string Street, string StreetNumber, string City, string State, string ZipCode)
        {
            var sc = new SportsCenter
            {
                Name = Name,
                Street = Street,
                StreetNumber = StreetNumber,
                City = City,
                State = State,
                ZipCode = ZipCode
            };

            _context.SportsCenters.Add(sc);
            _context.SaveChanges();
            return RedirectToPage();
        }

        //FACILITY/SPORTSOBJECT/////////////////////////////////////////////////////////////
        public IActionResult OnPostAddF(string FacilityName, string Name, string TypeName, bool IsChangingRoom, bool IsEquipment, DateTime PromoStart, DateTime PromoEnd, double PromoRate)
        {
            //Znalezienie SportsCenter na podstawie nazwy
           var sportsCenter = _context.SportsCenters.FirstOrDefault(sc => sc.Name == Name);
            if (sportsCenter == null)
            {
                ModelState.AddModelError(string.Empty, "Specified Sports Center not found.");
                return RedirectToPage();
            }

            //// Znalezienie FacilityType na podstawie typu (TypeName)
            var facilityType = _context.FacilityTypes.FirstOrDefault(ft => ft.TypeName == TypeName);
            if (facilityType == null)
            {
                ModelState.AddModelError(string.Empty, "Specified Facility Type not found.");
                return RedirectToPage();
            }

            //// Walidacja zakresu dat promocji
            //if (PromoEnd <= PromoStart)
            //{
            //    ModelState.AddModelError(string.Empty, "Promotion end date must be later than start date.");
            //    return RedirectToPage();
            //}

            // Tworzenie nowego obiektu Facility
            var facility = new Facility
            {
                FacilityName = FacilityName,
                FacilitySportsCenter = sportsCenter,
                FacilityType = facilityType,
                IsChangingRoom = IsChangingRoom,
                IsEquipment = IsEquipment,
                PromoStart = PromoStart,
                PromoEnd = PromoEnd,
                PromoRate = PromoRate
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

        public IActionResult OnPostEditF(int id, string FacilityName, string Name, string TypeName, bool IsChangingRoom, bool IsEquipment, DateTime PromoStart, DateTime PromoEnd, double PromoRate)
        {
            var fac = _context.Facilities
                              .Include(f => f.FacilitySportsCenter)
                              .Include(f => f.FacilityType)
                              .FirstOrDefault(f => f.FacilityId == id);

            if (fac != null)
            {
                fac.FacilityName = FacilityName;
                fac.FacilitySportsCenter.Name = Name;
                fac.FacilityType.TypeName = TypeName;
                fac.IsChangingRoom = IsChangingRoom;
                fac.IsEquipment = IsEquipment;
                fac.PromoStart = PromoStart;
                fac.PromoEnd = PromoEnd;
                fac.PromoRate = PromoRate;
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
            return RedirectToPage(); // Przekierowanie po usuni�ciu
        }

        public IActionResult OnPostEditUser(int id, string PassUserName, string PassUserSurName, Gender PassUserGender, string PassUserPhone, string PassUserEmail, string PassUserPassword)
        {
            var usr = _context.Users.Find(id);

            if (usr != null)
            {
                // Aktualizacja danych u�ytkownika
                usr.Name = PassUserName;
                usr.Surname = PassUserSurName;
                usr.DeclaredGender = PassUserGender;
                usr.PhoneNumber = PassUserPhone;
                usr.Email = PassUserEmail;

                // Sprawdzenie, czy has�o zosta�o podane
                if (!string.IsNullOrWhiteSpace(PassUserPassword))
                {
                    // Haszowanie has�a przed zapisem
                    var passwordHasher = new PasswordHasher<User>();
                    usr.Password = passwordHasher.HashPassword(null, PassUserPassword);
                }

                // Zapis zmian w bazie danych
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostAddUser(string PassUserName, string PassUserSurName, Gender PassUserGender, string PassUserPhone, string PassUserEmail, string PassUserPassword)
        {
            // Utw�rz instancj� PasswordHasher
            var passwordHasher = new PasswordHasher<User>();

            // Tworzenie u�ytkownika
            var usr = new User
            {
                Name = PassUserName,
                Surname = PassUserSurName,
                DeclaredGender = PassUserGender,
                PhoneNumber = PassUserPhone,
                Email = PassUserEmail,
                // Haszowanie has�a
                Password = passwordHasher.HashPassword(null, PassUserPassword)
            };

            // Dodanie u�ytkownika do bazy danych
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
            return RedirectToPage(); // Przekierowanie po usuni�ciu
        }

        public IActionResult OnPostEditWorker(int id, int workerFunction, string workerName, string workerSurName, Gender workerGender, string workerPhone, string workerEmail, string workerPassword)
        {
            // Pobranie funkcji pracownika
            var functionId = _context.WorkerFunctions.FirstOrDefault(fc => fc.WorkerFunctionId == workerFunction);
            var wrk = _context.Workers.Find(id);

            if (wrk != null)
            {
                // Aktualizacja danych pracownika
                wrk.AssignedWorkerFunction = functionId;
                wrk.Name = workerName;
                wrk.Surname = workerSurName;
                wrk.DeclaredGender = workerGender;
                wrk.PhoneNumber = workerPhone;
                wrk.Email = workerEmail;

                // Sprawdzenie, czy has�o zosta�o podane
                if (!string.IsNullOrWhiteSpace(workerPassword))
                {
                    // Haszowanie has�a przed zapisem
                    var passwordHasher = new PasswordHasher<Worker>();
                    wrk.Password = passwordHasher.HashPassword(null, workerPassword);
                }

                // Zapis zmian w bazie danych
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostAddWorker(int workerFunction, string workerName, string workerSurName, Gender workerGender, string workerPhone, string workerEmail, string workerPassword)
        {
            // Pobierz odpowiedni� funkcj� pracownika
            var functionId = _context.WorkerFunctions.FirstOrDefault(fc => fc.WorkerFunctionId == workerFunction);

            // Utw�rz hasher hase�
            var passwordHasher = new PasswordHasher<Worker>();

            // Utw�rz nowego pracownika
            var wrk = new Worker
            {
                AssignedWorkerFunction = functionId,
                Name = workerName,
                Surname = workerSurName,
                DeclaredGender = workerGender,
                PhoneNumber = workerPhone,
                Email = workerEmail,
                // Zhashowanie has�a przed zapisem
                Password = passwordHasher.HashPassword(null, workerPassword)
            };

            // Dodanie pracownika do bazy danych
            _context.Workers.Add(wrk);
            _context.SaveChanges();

            return RedirectToPage();
        }

    }
}
