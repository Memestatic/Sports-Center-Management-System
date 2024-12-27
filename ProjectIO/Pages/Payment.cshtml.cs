using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjectIO.Pages
{
    public class PaymentModel : PageModel
    {
        public int? reservationId;
        public void OnGet(int? reservationId)
        {
            this.reservationId = reservationId;
        }
    }
}
