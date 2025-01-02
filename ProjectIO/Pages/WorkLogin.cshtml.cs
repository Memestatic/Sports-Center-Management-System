using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectIO.Pages
{
    public class WorkLoginModel : PageModel
    {
        private readonly SportCenterContext _context;

        public WorkLoginModel(SportCenterContext context)
        {
            _context = context;
        }

        // Model do bindowania danych z formularza
        [BindProperty]
        public LoginInputModel Input { get; set; }

        // Komunikat o b��dzie
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Wyczyszczenie komunikatu o b��dzie przy za�adowaniu strony
            ErrorMessage = string.Empty;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Je�li dane s� niepoprawne, wr�� na stron�
            }

            // Wyszukiwanie u�ytkownika w bazie danych
            var worker = _context.Workers
                .FirstOrDefault(w => w.Email == Input.Email);

            if (worker == null)
            {
                /// !!! UWAGA - ODKOMENTOWA� ABY DODA� PIERWSZEGO ADMINISTRATORA !!! ///
                /// 

                //// Je�li u�ytkownik nie zosta� znaleziony, dodaj nowego pracownika
                //var passwordHasher = new PasswordHasher<AssignedWorker>();

                //var WorkerFunctionId = _context.WorkerFunctions.FirstOrDefault(fc => fc.WorkerFunctionId == 1);

                //worker = new AssignedWorker
                //{
                //    AssignedWorkerFunction = WorkerFunctionId,
                //    Email = Input.Email,
                //    Name = "John", // Wprowad� domy�ln� warto�� lub pobierz z formularza
                //    Surname = "Doe", // Wprowad� domy�ln� warto�� lub pobierz z formularza
                //    DeclaredGender = Gender.Male, // Domy�lna warto�� dla p�ci, je�li brak w formularzu
                //    PhoneNumber = "123456789", // Domy�lny numer telefonu
                //    Password = passwordHasher.HashPassword(null, Input.Password) // Hashuj has�o
                //};

                //_context.Workers.Add(worker);
                //_context.SaveChanges();

                //// Dodaj komunikat informacyjny
                //ErrorMessage = "New worker created. Please log in again.";
                return Page();
            }

            var passwordHasherForVerification = new PasswordHasher<Worker>();
            var result = passwordHasherForVerification.VerifyHashedPassword(worker, worker.Password, Input.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Input.Password", "The Password is incorrect.");
                return Page();
            }

            CurrentPerson.SetInstance(worker); // Ustawienie zalogowanego u�ytkownika

            return RedirectToPage("/AdminPanel");
        }

    }

}
