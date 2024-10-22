using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TripEnjoy.Presentation.Razor.Pages.Home
{
    public class _404Model : PageModel
    {
        public IActionResult OnGet()
        {
            var temp = Request.Query["message"];
            ViewData["message"] = temp;
            return Page();
        }
    }
}
