using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;

namespace ProjectIO.Pages
{
    public class PricesModel : PageModel
    {
        public List<SportsCenter> SportsCenters { get; set; }
        public List<Facility> Facilities { get; set; }

        private readonly SportCenterContext _context;
        public PricesModel(SportCenterContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            SportsCenters = _context.SportsCenters.ToList() ?? new List<SportsCenter>();
            Facilities = _context.Facilities.ToList();

            return Page();
        }
    }
}
