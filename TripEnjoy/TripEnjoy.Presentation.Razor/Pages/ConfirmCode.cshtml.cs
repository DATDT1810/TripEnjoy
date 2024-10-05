using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class ConfirmCodeModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public ConfirmCodeModel(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public string Code { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Password { get; set; }

        public string Message { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Email = Request.Query["email"];
            Password = Request.Query["newPassword"];

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/SendCode");
            var content = new
            {
                Email = Email
            };
            request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                
                return Page();
            }
            Message = "Error";
            return Page();
        }   

        public async Task<IActionResult> OnPost()
        {
            var  changePassword = new ChangePassword
            {
                password = Password,
                code = Code,
                email = Email
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/ResetPassword");
            request.Content = new StringContent(JsonConvert.SerializeObject(changePassword), Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }
    }
}
