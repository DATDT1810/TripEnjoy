using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public LoginModel(IHttpClientFactory clientFactory)
        {
          this._clientFactory = clientFactory;

        }

        // binding dữ liệu
        [BindProperty(SupportsGet = true)]
        public LoginViewModel LoginViewModel { get; set; }

        public async Task<IActionResult> OnGet()
        {

            return Page();

        }
        public async Task<IActionResult> OnPostAsync(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/Login");
                request.Content = new StringContent(JsonConvert.SerializeObject(loginViewModel), Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient();
                try
                {
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var token = await response.Content.ReadAsStringAsync();

                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            Expires = DateTime.UtcNow.AddDays(1),
                            SameSite = SameSiteMode.Strict,
                        };

                        Response.Cookies.Append("token", token, cookieOptions);

                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }
                catch (HttpRequestException e)
                {
                    ModelState.AddModelError(string.Empty, "Error connecting to the login service.");
                    return Page();
                }
            }
            return Page();
        }
    }

}
