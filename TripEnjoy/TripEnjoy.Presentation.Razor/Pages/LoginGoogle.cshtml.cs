using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class LoginGoogleModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginGoogleModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGet()
        {
            var result = await HttpContext.AuthenticateAsync();

            if (!result.Succeeded)
            {
                return RedirectToPage("/Error");
            }

            // Trích xuất thông tin từ Google
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var googleUserEmail = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Gửi thông tin này tới ASP.NET API để xác thực với Identity
            if (googleUserEmail != null)
            {
                var token = await AuthenticateWithApi(googleUserEmail);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SameSite = SameSiteMode.Lax,
                };
                Response.Cookies.Append("accessToken", token.AccessToken, cookieOptions);
                Response.Cookies.Append("refreshToken", token.RefreshToken, cookieOptions);

                await Task.Delay(500);
               
               
            }

            return RedirectToPage("/Index");
        }

        public async Task<TokenResponse> AuthenticateWithApi(string googleEmail)
        {
            // Gửi yêu cầu tới API để xác thực
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/LoginGoogle");
            request.Content = new StringContent(JsonConvert.SerializeObject(new {email = googleEmail }), Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            try
            {
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(token);
                    if (tokenResponse != null)
                    {
                        return tokenResponse;
                    }
                }
                throw new Exception("Invalid login attempt.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return null;
            }
        }
    }
}
