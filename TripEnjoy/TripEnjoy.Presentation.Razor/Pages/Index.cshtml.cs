using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;
using TripEnjoy.Presentation.Razor.Services;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenServices _tokenServices;
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Hotel> Hotels { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory , TokenServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _tokenServices = tokenServices;
        }

        public async Task<IActionResult> OnGet(string location = null)
        {
            var token = _tokenServices.GetAccessToken(); //accessToken
            if (!string.IsNullOrEmpty(token))
            {
                ViewData["token"] = "User";
            }
            // Call API to get the list of hotels
            var client = _httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var response = await client.GetAsync("https://localhost:7126/api/Hotel");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Hotels = JsonConvert.DeserializeObject<List<Hotel>>(data);
            }
            else
            {
                Hotels = new List<Hotel>();
            }
            return Page();
        }
    }
}
