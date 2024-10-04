using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class IndexModel : PageModel
    {
        
        public IActionResult OnGet()
        {
            var token = Request.Cookies["accessToken"]; //accessToken
            if (!string.IsNullOrEmpty(token))
            {
                ViewData["token"] = "User";
            }
            return Page();
        }
    }
}
