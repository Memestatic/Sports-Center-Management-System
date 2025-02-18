using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectIO.model;
namespace ProjectIO.Pages.Account;

public class ChoosePaymentMethod : PageModel
{
    private readonly SportCenterContext _context;
    
    [BindProperty]
    public List<Pass> Passes { get; set; } = new List<Pass>(); 
    
    [BindProperty]
    public string OrderId { get; set; }
    
    public ChoosePaymentMethod(SportCenterContext context)
    {
        _context = context;
    }
    
    public IActionResult OnGet(string orderId)
    {
        OrderId = orderId;
        int? id = HttpContext.Session.GetInt32("userID");
            
        if (id == null)
        {
            return RedirectToPage("/Account/Login");
        }
        
        User user = _context.Users.FirstOrDefault(u => u.UserId == id);
        
        Passes = _context.Passes
            .Where(p => p.PassUser.UserId == user.UserId)
            .Where(p => p.CurrentStatus == Status.Approved)
            .Include(p => p.PassType)
            .ToList();
        return Page();
    }

    public IActionResult OnPostQuickPayment()
    {
        return RedirectToPage("/Payment", new { orderId = OrderId });
    }

    public IActionResult OnPostPayWithPass()
    {
        int PassId = int.Parse(Request.Form["selected"]);
        
        Pass pass = _context.Passes.First(p => p.PassId == PassId);
        
        int ResId = int.Parse(OrderId.Substring(1));
        
        Reservation reservation = _context.Reservations.First(r => r.ReservationId == ResId);
        
        if (pass == null)
        {
            return NotFound("Karnet nie znaleziony");
        }

        if (reservation == null)
        {
            return NotFound("Rezerwacja nie znaleziona");
        }

        if (reservation.CurrentStatus != Status.Pending)
        {
            return NotFound("Rezerwacja nie jest w stanie oczekującym");
        }
        
        reservation.CurrentStatus = Status.Approved;
        
        if (pass.PassEntriesLeft < 1)
        {
            //_context.Passes.Remove(pass);
            pass.CurrentStatus = Status.Denied;
        }
        else
        {
            pass.PassEntriesLeft--;
        }
        
        _context.SaveChanges();
        return RedirectToPage("/Account/ClientPanel");
    }
}