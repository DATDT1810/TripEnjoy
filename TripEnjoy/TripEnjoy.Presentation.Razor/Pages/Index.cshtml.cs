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

        [BindProperty(SupportsGet = true)]
        public IEnumerable<HotelImages> HotelImages { get; set; }
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
                Hotels = JsonConvert.DeserializeObject<List<Hotel>>(data);
            }

            // Call API to get all hotel images
            var imagesResponse = await client.GetAsync("https://localhost:7126/api/HotelImage");
            if (imagesResponse.IsSuccessStatusCode)
            {
                string imagesData = await imagesResponse.Content.ReadAsStringAsync();
                HotelImages = JsonConvert.DeserializeObject<List<HotelImages>>(imagesData);
            }

            // Call API to get the list of categories
            var categoryResponse = await client.GetAsync("https://localhost:7126/api/Category");

            if (categoryResponse.IsSuccessStatusCode)
            {
                string categoryData = await categoryResponse.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<Category>>(categoryData);

                // Map the category names to the hotels based on CategoryId
                foreach (var hotel in Hotels)
                {
                    hotel.Category = categories.FirstOrDefault(c => c.CategoryId == hotel.CategoryId);
                }
            }
            return Page();
        }
    }
}
