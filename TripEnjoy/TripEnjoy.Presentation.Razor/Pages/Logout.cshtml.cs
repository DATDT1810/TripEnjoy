using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        public LogoutModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<IActionResult> OnGet()
        {
            var accessToken = Request.Cookies["accessToken"];
            if (accessToken != null)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/Logout");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var client = httpClientFactory.CreateClient();
                client.Timeout = TimeSpan.FromMinutes(2);
                try
                {
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        Response.Cookies.Delete("accessToken");
                        Response.Cookies.Delete("refreshToken");
                        return RedirectToPage("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Error when logging out.");
                        return RedirectToPage("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    return RedirectToPage("Index");
                }
            }
            return RedirectToPage("Index");
        }
    }
}
