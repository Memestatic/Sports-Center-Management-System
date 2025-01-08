using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectIO.model;
namespace ProjectIO.Pages.Account;

public class BuyPass : PageModel
{
    private readonly SportCenterContext _context;
    
    [BindProperty]
    public List<PassType>? PassTypes { get; set; }
    
    [BindProperty]
    public int PassTypeId { get; set; }

    public BuyPass(SportCenterContext context)
    {
        _context = context;
    }
    
    public IActionResult OnGet()
    {
        int? id = HttpContext.Session.GetInt32("userID");
            
        if (id == null)
        {
            return RedirectToPage("/Account/Login");
        }
        
        PassTypes = _context.PassTypes.ToList();
        return Page();
    }

    public IActionResult OnPost()
    {
        int? id = HttpContext.Session.GetInt32("userID");
            
        if (id == null)
        {
            return RedirectToPage("/Account/Login");
        }
        
        User user = _context.Users.FirstOrDefault(u => u.UserId == id);
        PassType Chosen = _context.PassTypes.FirstOrDefault(p => p.PassTypeId == PassTypeId);
        Pass UserPass = new Pass()
        {
            PassUser = user,
            PassType = Chosen,
            PassEntriesLeft = Chosen.PassTypeDuration,
            CurrentStatus = Status.Pending
        };
        
        _context.Passes.Add(UserPass);
        _context.SaveChanges();
        return RedirectToPage("/Payment", new {OrderId = "p" + UserPass.PassId.ToString()});
        
    }
}