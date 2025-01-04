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
        if (CurrentPerson.GetInstance() == null)
        {
            return RedirectToPage("/Login");
        }
        
        PassTypes = _context.PassTypes.ToList();
        return Page();
    }

    public IActionResult OnPost()
    {
        User user = (User)CurrentPerson.GetInstance();
        PassType Chosen = _context.PassTypes.FirstOrDefault(p => p.PassTypeId == PassTypeId);
        _context.Attach(user);
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