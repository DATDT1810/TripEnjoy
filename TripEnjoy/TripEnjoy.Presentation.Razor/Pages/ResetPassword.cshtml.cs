using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public ResetPasswordModel(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }
        [BindProperty(SupportsGet = true)]
        public ResetPassword ResetPassword { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }
        public IActionResult OnGet()
        {
            Email = Request.Query["email"];
            return Page();
        }
        public async Task<IActionResult> OnPost(ResetPassword resetPassword)
        {
            if (ModelState.IsValid)
            {
                return RedirectToPage("/ConfirmCode", new {email = Email , newPassword =  ResetPassword.password});
            }
            return Page();
        }
    }
}
