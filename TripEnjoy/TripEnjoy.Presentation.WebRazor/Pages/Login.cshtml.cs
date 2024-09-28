using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TripEnjoy.Presentation.WebRazor.ViewModels;

namespace TripEnjoy.Presentation.WebRazor.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient httpClient;
        public LoginModel(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        [BindProperty(SupportsGet = true)]
        public LoginViewModel LoginViewModel { get; set; }

        public List<LoginViewModel> List = new List<LoginViewModel>();

        public async Task<IActionResult> OnGet()
        {
            return Page();

        }
        public async Task<IActionResult> OnPostAsync()
        {
            List.Add(LoginViewModel);
            return Page();
        }

    }
}
