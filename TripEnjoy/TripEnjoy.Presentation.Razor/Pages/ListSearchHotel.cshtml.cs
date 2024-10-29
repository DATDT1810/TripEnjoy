using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TripEnjoy.Presentation.Razor.Model;
using TripEnjoy.Presentation.Razor.Services;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class ListSearchHotelModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly TokenServices _tokenServices;
        
        public List<Hotel> ListSearchHotel { get; set; } = new List<Hotel>();
        public ListSearchHotelModel(IHttpClientFactory httpClientFactory, TokenServices tokenServices)
        {
            _httpClientFactory = httpClientFactory;
            _tokenServices = tokenServices;
        }

        public async Task<IActionResult> OnGet()
        {
            if (TempData["ListSearchHotel"] != null)
            {
               var hothotelsJson = TempData["ListSearchHotel"].ToString();
                ListSearchHotel = JsonSerializer.Deserialize<List<Hotel>>(hothotelsJson);
                Console.WriteLine(ListSearchHotel);
            }
            return Page();
        }
    }
}
