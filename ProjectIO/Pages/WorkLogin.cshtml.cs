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

        // Komunikat o b³êdzie
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
            // Wyczyszczenie komunikatu o b³êdzie przy za³adowaniu strony
            ErrorMessage = string.Empty;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Jeœli dane s¹ niepoprawne, wróæ na stronê
            }

            // Wyszukiwanie u¿ytkownika w bazie danych
            var worker = _context.Workers
                .FirstOrDefault(w => w.email == Input.email);

            if (worker == null)
            {
                /// !!! UWAGA - ODKOMENTOWAÆ ABY DODAÆ PIERWSZEGO ADMINISTRATORA !!! ///
                /// 

                //// Jeœli u¿ytkownik nie zosta³ znaleziony, dodaj nowego pracownika
                //var passwordHasher = new PasswordHasher<Worker>();

                //var functionId = _context.WorkerFunctions.FirstOrDefault(fc => fc.functionId == 1);

                //worker = new Worker
                //{
                //    function = functionId,
                //    email = Input.email,
                //    name = "John", // WprowadŸ domyœln¹ wartoœæ lub pobierz z formularza
                //    surname = "Doe", // WprowadŸ domyœln¹ wartoœæ lub pobierz z formularza
                //    gender = Gender.Male, // Domyœlna wartoœæ dla p³ci, jeœli brak w formularzu
                //    phone = "123456789", // Domyœlny numer telefonu
                //    password = passwordHasher.HashPassword(null, Input.password) // Hashuj has³o
                //};

                //_context.Workers.Add(worker);
                //_context.SaveChanges();

                //// Dodaj komunikat informacyjny
                //ErrorMessage = "New worker created. Please log in again.";
                return Page();
            }

            var passwordHasherForVerification = new PasswordHasher<Worker>();
            var result = passwordHasherForVerification.VerifyHashedPassword(worker, worker.password, Input.password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Input.password", "The password is incorrect.");
                return Page();
            }

            CurrentPerson.SetInstance(worker); // Ustawienie zalogowanego u¿ytkownika

            return RedirectToPage("/AdminPanel");
        }

    }

}
