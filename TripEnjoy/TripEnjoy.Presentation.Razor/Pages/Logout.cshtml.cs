using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using TripEnjoy.Presentation.Razor.Services;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly TokenServices tokenServices;

        public LogoutModel(IHttpClientFactory httpClientFactory, TokenServices tokenServices)
        {
            this.httpClientFactory = httpClientFactory;
            this.tokenServices = tokenServices;
        }
        public async Task<IActionResult> OnGet()
        {
 
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/Logout");
               
                var client = httpClientFactory.CreateClient("DefaultClient");
                client.Timeout = TimeSpan.FromMinutes(2);
                try
                {
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                       
                        foreach(var cookie in Request.Cookies.Keys)
                        {
                            Response.Cookies.Delete(cookie);
                        }
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
    }
}
