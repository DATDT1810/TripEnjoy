using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using TripEnjoy.Presentation.Razor.Services;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly TokenServices _tokenServices;
        public LoginModel(IHttpClientFactory clientFactory, TokenServices tokenServices)
        {
            this._clientFactory = clientFactory;
            this._tokenServices = tokenServices;
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
                client.Timeout = TimeSpan.FromMinutes(2);
                try
                {
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        var token = await response.Content.ReadAsStringAsync();
                        var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(token);
                        if (tokenResponse?.AccessToken == null || tokenResponse?.RefreshToken == null)
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                            return Page();
                        }

                        _tokenServices.SetTokens(tokenResponse.AccessToken, tokenResponse.RefreshToken);

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
                    ModelState.AddModelError(string.Empty, e.Message);
                    return Page();
                }
            }
            return RedirectToPage("/Index");
        }

        public IActionResult OnGetLoginWithGoogle()
        {
            var redirectUrl = Url.Page("/LoginGoogle");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return new ChallengeResult("Google", properties);
        }

    }
}
