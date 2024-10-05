using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public ForgetPasswordModel(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }
        [BindProperty(SupportsGet = true)]
        public EmailConfirm EmailConfirm { get; set; } 

       
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost(EmailConfirm emailConfirm)
        {
            if (ModelState.IsValid)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/CheckEmail");
                request.Content = new StringContent(JsonConvert.SerializeObject(emailConfirm), Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/ResetPassword" , new {email = emailConfirm.Email });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email");
                    return Page();
                }
            }
                return Page();
        }
    }
}
