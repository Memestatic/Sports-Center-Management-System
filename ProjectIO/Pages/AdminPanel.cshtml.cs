using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Asn1.Esf;
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

        public List<WorkerTrainingSession> WorkerTrainingSessions { get; set; }

        public List<WorkerTrainingSession> WorkerTrainingSessionsDay { get; set; }
        public List<TrainingSession> TrainingSessions { get; set; }

        public string ActiveTab { get; set; }
        public int Permissions { get; set; }

        public int ActiveUserId { get; set; }

        [BindProperty(SupportsGet = true)] public string SelectedDay { get; set; }
        [BindProperty(SupportsGet = true)] public int? SelectedCenterId { get; set; }
        [BindProperty(SupportsGet = true)] public string? SelectedCenterName { get; set; }
        [BindProperty(SupportsGet = true)] public int? SelectedObjectId { get; set; }

        public List<string> TakenSlots { get; set; } = new List<string>();

        public Facility selectedObject { get; set; }

        public string? SelectedObjectName { get; set; }

        // Zakres dat do wyboru w kalendarzu
        public string MinDate => DateTime.Now.ToString("yyyy-MM-dd");
        public string MaxDate => DateTime.Now.AddDays(30).ToString("yyyy-MM-dd");

        public List<int> OccupiedHours { get; set; } = new List<int>();

        [BindProperty]
        public Worker perms { get; set; }

        private readonly SportCenterContext _context;

        private readonly IConfiguration _configuration;



        public bool redirect;
        public AdminPanel(SportCenterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public TrainingSession EditingSession { get; set; }
        public WorkerTrainingSession EditingWorkerSession { get; set; }

        // TESY /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        [BindProperty(SupportsGet = true)]
        public string IsAddFormVisible { get; set; }
        public int SelectedHour { get; set; }
        public string PlaceholderMessage { get; set; } // Pole na placeholder



        public void OnPostChangeFormState(int? centerId, int? objectId, string selectedDay, int selectedHour)
        {
            InitializeData(centerId, objectId, "workerSession");
            TempData["SelectedDay"] = selectedDay;
            TempData["SelectedHour"] = selectedHour;
            TempData["IsAddFormVisible"] = "Adding";
            OnGet(centerId, objectId, "workerSession");


        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void LoadDataForSelectedDate(int? objectId)
        {

            if (TempData["SelectedDay"] != null)
            {
                SelectedDay = TempData["SelectedDay"].ToString();
            }

            if (TempData["SelectedHour"] != null)
            {
                SelectedHour = (int)TempData["SelectedHour"];
            }

            if (TempData["IsAddFormVisible"] != null)
            {
                IsAddFormVisible = TempData["IsAddFormVisible"].ToString();
            }
            // Pobierz dane powiązane z wybraną datą
            var date = DateTime.Parse(SelectedDay);

            //WorkerTrainingSessionsDay = _context.WorkerTrainingSessions
            //    .Include(wts => wts.TrainingSession)         // Ładowanie TrainingSession
            //    .Include(wts => wts.TrainingSession.Facility)          // Ładowanie Facility powiązanej z TrainingSession
            //    .Where(wts => wts.TrainingSession.Date.Date == date.Date 
            //                  && wts.TrainingSession.Facility.FacilityId == objectId) // Filtrowanie po FacilityId
            //    .ToList();

            //// Przypisz godziny rozpoczęcia do listy OccupiedHours
            //OccupiedHours = WorkerTrainingSessionsDay
            //        .Select(wts => wts.TrainingSession.Date.Hour)
            //        .Distinct()
            //        .ToList();
        }

        private void updateTakenSlots(string selectedDay)
        {
            if (!string.IsNullOrEmpty(selectedDay) && SelectedObjectId.HasValue)
            {
                DateTime selectedDate;
                if (DateTime.TryParse(selectedDay, out selectedDate))
                {
                    foreach (var reservation in Reservations)
                    {
                        if (reservation.ReservationFacility.FacilitySportsCenter.SportsCenterId == SelectedCenterId &&
                            reservation.ReservationFacility.FacilityId == SelectedObjectId &&
                            reservation.ReservationDate.Date == selectedDate.Date &&
                            reservation.CurrentStatus == Status.Approved)
                        {
                            string slot = $"{reservation.ReservationFacility.FacilitySportsCenter.SportsCenterId} {reservation.ReservationFacility.FacilityId} {reservation.ReservationDate:yyyy-MM-dd HH} U";
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
                            string slot = $"{session.Facility.FacilitySportsCenter.SportsCenterId} {session.Facility.FacilityId} {session.Date:yyyy-MM-dd HH} W";
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
                            string pastSlot = $"{SelectedCenterId} {SelectedObjectId} {selectedDate:yyyy-MM-dd} {pastHour:00} T";
                            if (!TakenSlots.Contains(pastSlot))
                            {
                                TakenSlots.Add(pastSlot);
                            }
                        }
                    }
                }
            }
        }

        private void InitializeData(int? centerId, int? objectId, string tab)
        {
            LoadDataForSelectedDate(objectId);

            SportsCenters = _context.SportsCenters.ToList();
            Facilities = _context.Facilities.ToList();
            FacilityTypes = _context.FacilityTypes.ToList();
            Users = _context.Users.ToList();
            Workers = _context.Workers.ToList();
            Reservations = _context.Reservations.ToList();
            WorkerFunctions = _context.WorkerFunctions.ToList();
            WorkerTrainingSessions = _context.WorkerTrainingSessions.ToList();
            TrainingSessions = _context.TrainingSessions.ToList();
            SelectedCenterId = centerId;
            SelectedObjectId = objectId;
            ActiveTab = "workerSession";

            // Obs�uga wybranego dnia
            updateTakenSlots(SelectedDay);


        }

        public void OnPostChangeDate(string selectedDay, int selectedCenterId, int? selectedObjectId)
        {
            PlaceholderMessage = null; // Resetuj placeholder

            SelectedDay = selectedDay;

            InitializeData(selectedCenterId, selectedObjectId, "workerSession");

            // Załaduj dane dla wybranej daty
            LoadDataForSelectedDate(selectedObjectId);

            
        }

        public IActionResult OnPostEditFormState(int? centerId, int? objectId, string selectedDay, int selectedHour)
        {
            InitializeData(centerId, objectId, "workerSession");

            SelectedDay = selectedDay;
            SelectedHour = selectedHour;
            var parsedDate = DateTime.Parse(SelectedDay); // Parsuj SelectedDay na obiekt DateTime
            // Załaduj istniejącą sesję treningową dla podanych danych
            var session = _context.WorkerTrainingSessions
                .Include(wts => wts.TrainingSession)         // Ładowanie TrainingSession
                //.Include(wts => wts.TrainingSession.Facility)          // Ładowanie Facility powiązanej z TrainingSession
                .Include(wts => wts.AssignedWorker)          // Ładowanie AssignedWorker
                .Where(wts => wts.TrainingSession.Date.Date == parsedDate.Date // Filtruj po dniu
                              && wts.TrainingSession.Date.Hour == SelectedHour // Filtruj po godzinie
                              && wts.TrainingSession.Facility.FacilityId == objectId) // Filtruj po FacilityId
                .FirstOrDefault(); // Pobierz pierwszy pasujący obiekt lub null

            if (session != null)
            {
                //EditingSession = new TrainingSession
                //{
                //    Facility = session.TrainingSession.Facility,
                //    Name = session.TrainingSession.Name,
                //    Date = session.TrainingSession.Date,
                //    Duration = session.TrainingSession.Duration,
                //    GroupCapacity = session.TrainingSession.GroupCapacity,
                //};
                EditingWorkerSession = new WorkerTrainingSession
                {
                    TrainingSession = session.TrainingSession,
                    AssignedWorker = session.AssignedWorker,
                    SessionId = session.TrainingSession.TrainingSessionId,
                    AssignedWorkerId = session.AssignedWorker.WorkerId
                };

            }

            IsAddFormVisible = "Edit"; // Zmień stan na edytowalny
            OnGet(centerId, objectId, "workerSession"); // Załaduj dane potrzebne do widoku
            return Page();
        }



        public IActionResult OnGet(int? centerId, int? objectId, string tab)
        {

            int? id = HttpContext.Session.GetInt32("workerID");
            if (id != null)
            {
                perms = _context.Workers.FirstOrDefault(w => w.WorkerId == id);
                redirect = false;
            }
            else
            {
                redirect = true;
            }

            if (redirect)
            {
                return Redirect("/WorkLogin");
            }
            
            SelectedDay = SelectedDay ?? DateTime.Now.ToString("yyyy-MM-dd");
            LoadDataForSelectedDate(objectId);
            

            if (HttpContext.Session.GetInt32("userID") != null)
            {
                return BadRequest("Login into your worker account first");
            }



            Permissions = _context.Workers
               .Where(w => w.WorkerId == perms.WorkerId)
               .Select(w => w.AssignedWorkerFunction.WorkerFunctionId)
               .FirstOrDefault();

            ActiveUserId = perms.WorkerId;

            SportsCenters = _context.SportsCenters.ToList();
            Facilities = _context.Facilities.ToList();
            FacilityTypes = _context.FacilityTypes.ToList();
            Users = _context.Users.ToList();
            Workers = _context.Workers.ToList();
            Reservations = _context.Reservations.ToList();
            WorkerFunctions = _context.WorkerFunctions.ToList();
            WorkerTrainingSessions = _context.WorkerTrainingSessions.ToList();
            TrainingSessions = _context.TrainingSessions.ToList();

            if (!string.IsNullOrEmpty(tab))
            {
                ActiveTab = tab;
            }
            if(tab == "workerSession")
            {
                InitializeData(centerId, objectId, tab);
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
                    HttpContext.Session.SetInt32("SelectedObjectId", (int)SelectedObjectId);

                }
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
        public async Task<IActionResult> OnPostAddF(string FacilityName, double Price, string Name, string TypeName, bool IsChangingRoom, bool IsEquipment, DateTime PromoStart, DateTime PromoEnd, double PromoRate)
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
                PromoRate = PromoRate,
                Price = Price
            };

            // Dodanie nowego Facility do bazy danych
            _context.Facilities.Add(facility);
            _context.SaveChanges();

            var now = DateTime.Now;
            if (now >= PromoStart && now <= PromoEnd)
            {
                Marketing marketing = new Marketing(this._context, this._configuration);
                await marketing.SendMarketingEmails(facility);
            }

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

        public IActionResult OnPostEditF(int id, double Price, string FacilityName, string Name, string TypeName, bool IsChangingRoom, bool IsEquipment, DateTime PromoStart, DateTime PromoEnd, double PromoRate)
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
                fac.Price = Price;
                _context.SaveChanges();

                var now = DateTime.Now;
                if (now >= PromoStart && now <= PromoEnd)
                {
                    Marketing marketing = new Marketing(this._context, this._configuration);
                    marketing.SendMarketingEmails(fac);
                }
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
                usr.IsActive = true;

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
                Password = passwordHasher.HashPassword(null, PassUserPassword),
                IsActive = true
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

        public IActionResult OnPostAddReservation(int facilityId, int userId, DateTime reservationDate, string status, bool isChangingRoomReserved, bool isEquipmentReserved)
        {
            Enum.TryParse(status, out Status statusS);
            // Tworzenie nowej rezerwacji
            var reservation = new Reservation
            {
                ReservationFacility = _context.Facilities.Find(facilityId),
                ReservationUser = _context.Users.Find(userId),
                ReservationDate = reservationDate,
                CurrentStatus = statusS,
                IsChangingRoomReserved = isChangingRoomReserved,
                IsEquipmentReserved = isEquipmentReserved
            };

            // Dodanie do bazy danych
            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostEditReservation(int id, int facilityId, int userId, DateTime reservationDate, string status, bool isChangingRoomReserved, bool isEquipmentReserved)
        {
            // Pobranie istniejącej rezerwacji
            var reservation = _context.Reservations.Find(id);
            Enum.TryParse(status, out Status statusS);
            if (reservation != null)
            {
                reservation.ReservationFacility = _context.Facilities.Find(facilityId);
                reservation.ReservationUser = _context.Users.Find(userId);
                reservation.ReservationDate = reservationDate;
                reservation.CurrentStatus = statusS;
                reservation.IsChangingRoomReserved = isChangingRoomReserved;
                reservation.IsEquipmentReserved = isEquipmentReserved;

                // Zapisanie zmian
                _context.SaveChanges();
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteReservation(int id)
        {
            // Znalezienie rezerwacji
            var reservation = _context.Reservations.Find(id);
            if (reservation != null)
            {
                // Usunięcie rezerwacji
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
            }

            return RedirectToPage();
        }


        public IActionResult OnPostAddTrainingSession(int FacilityId, string Name, DateTime Date, int Duration, int GroupCapacity, int TrainerId)
        {
            // Pobierz obiekt Facility z bazy danych
            var facility = _context.Facilities.FirstOrDefault(f => f.FacilityId == FacilityId);
            if (facility == null)
            {
                ModelState.AddModelError("", "Invalid Facility selected.");
                return Page();
            }

            // Pobierz obiekt Worker (trenera) z bazy danych na podstawie TrainerId
            var trainer = _context.Workers
                .FirstOrDefault(w => w.WorkerId == TrainerId && w.AssignedWorkerFunction.WorkerFunctionId == 2);
            if (trainer == null)
            {
                ModelState.AddModelError("", "Invalid Trainer selected.");
                return Page();
            }
            var aaa = new TrainingSession
            {
                Facility = facility,
                Name = Name,
                Date = Date,
                GroupCapacity = GroupCapacity
            };
            // Utwórz nową sesję treningową
            var trainingSession = new WorkerTrainingSession
            {
                AssignedWorker = trainer,
                TrainingSession = aaa,
                SessionId = aaa.TrainingSessionId,
                AssignedWorkerId = trainer.WorkerId

            };

            // Dodaj sesję treningową do bazy danych
            _context.TrainingSessions.Add(aaa);
            _context.WorkerTrainingSessions.Add(trainingSession);
            _context.SaveChanges();

            return RedirectToPage();
        }

        public IActionResult OnPostEditTrainingSession(int id, int FacilityId, string Name, DateTime Date, int Duration, int GroupCapacity, int TrainerId)
        {
            // Znajdź istniejącą sesję w bazie danych wraz z powiązaną placówką (Facility)
            var trainingSessionId = id;

            var trainingSession = _context.TrainingSessions
                .Include(ts => ts.Facility)
                .FirstOrDefault(ts => ts.TrainingSessionId == trainingSessionId);


            if (trainingSession == null)
            {
                return NotFound("Training session not found.");
            }

            // Znajdź powiązaną relację w WorkerTrainingSessions wraz z przypisanym pracownikiem i sesją
            var workerTrainingSession = _context.WorkerTrainingSessions
                .Include(wts => wts.AssignedWorker)  // Ładowanie przypisanego pracownika
                .Include(wts => wts.TrainingSession) // Ładowanie powiązanej sesji treningowej
                .FirstOrDefault(wts => wts.SessionId == trainingSession.TrainingSessionId);

            if (workerTrainingSession == null)
            {
                return NotFound("WorkerTrainingSession not found.");
            }

            // Aktualizacja danych sesji treningowej
            trainingSession.Facility = _context.Facilities.FirstOrDefault(f => f.FacilityId == FacilityId)
                                        ?? throw new InvalidOperationException("Facility not found.");
            trainingSession.Name = Name;
            trainingSession.Date = Date;
            trainingSession.GroupCapacity = GroupCapacity;

            // Aktualizacja przypisanego trenera
            workerTrainingSession.AssignedWorker = _context.Workers
                .Include(w => w.AssignedWorkerFunction) // Ładowanie funkcji przypisanego pracownika
                .FirstOrDefault(w => w.WorkerId == TrainerId)
                ?? throw new InvalidOperationException("Trainer not found.");

            // Zapisz zmiany w bazie danych
            _context.SaveChanges();

            // Przekierowanie po zapisaniu zmian
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteWorkerTrainingSession(int sessionid, int workerid)
        {
            var workerTrainingSession = _context.WorkerTrainingSessions
                .FirstOrDefault(wts => wts.SessionId == sessionid && wts.AssignedWorkerId == workerid);

            var TrainingSession = _context.TrainingSessions
                .FirstOrDefault(wts => wts.TrainingSessionId == sessionid);



            if (workerTrainingSession != null)
            {
                _context.WorkerTrainingSessions.Remove(workerTrainingSession);
                _context.SaveChanges();
            }

            if (TrainingSession != null)
            {
                _context.TrainingSessions.Remove(TrainingSession);
                _context.SaveChanges();
            }

            return RedirectToPage(); // Przekierowanie po usunięciu
        }

    }
}
