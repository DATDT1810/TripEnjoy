using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TripEnjoy.Presentation.Razor.Pages.Payment
{
    public class SuccessModel : PageModel
    {
      

        public IActionResult OnGet()
        {
            var temp = Request.Query["message"];
            ViewData["message"] = temp;
            return Page();
        }
    }
}
