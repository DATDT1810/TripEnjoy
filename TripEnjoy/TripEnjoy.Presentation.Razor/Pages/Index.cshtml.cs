using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Hotel> Hotels { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<HotelImages> HotelImages { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Call API to get the list of hotels
            var client = _httpClientFactory.CreateClient();
            var hotelResponse = await client.GetAsync("https://localhost:7126/api/Hotel");
            if (hotelResponse.IsSuccessStatusCode)
            {
                string data = await hotelResponse.Content.ReadAsStringAsync();
                Hotels = JsonConvert.DeserializeObject<List<Hotel>>(data);
            }

            // Call API to get all hotel images
            var imagesResponse = await client.GetAsync("https://localhost:7126/api/HotelImage");
            if (imagesResponse.IsSuccessStatusCode)
            {
                string imagesData = await imagesResponse.Content.ReadAsStringAsync();
                HotelImages = JsonConvert.DeserializeObject<List<HotelImages>>(imagesData);
            }

            return Page();
        }
    }
}
