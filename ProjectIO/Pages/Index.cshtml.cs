using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIO.model;

namespace ProjectIO.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<SportsCenter> SportsCenters { get; set; }

        private readonly SportCenterContext _context;

        public IndexModel(SportCenterContext context)
        {
            _context = context;
        }

        public void OnGet()
        {

            SportsCenters = SportsCenters = _context.SportsCenters.ToList() ?? new List<SportsCenter>();
        }
    }
}
