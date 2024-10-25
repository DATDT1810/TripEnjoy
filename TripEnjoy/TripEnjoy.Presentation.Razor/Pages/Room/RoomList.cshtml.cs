using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Room
{
    public class RoomListModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty(SupportsGet = true)]
        public IEnumerable<RoomVM> RoomVM { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<RoomImages> RoomImages { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<Rate> Rates { get; set; }

        [BindProperty(SupportsGet = true)]
        public Hotel Hotel { get; set; }

        public RoomListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int hotelId)
        {
            if(hotelId == 0)
            {
                return NotFound();
            }
            
            var client = _httpClientFactory.CreateClient("DefaultClient");

            // gọi api của hotel, lấy hotel
            var hotelResponse = await client.GetAsync($"https://localhost:7126/api/Hotel/{hotelId}");
            if (hotelResponse.IsSuccessStatusCode)
            {
                string hotelData = await hotelResponse.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Hotel = JsonSerializer.Deserialize<Hotel>(hotelData, option);

                var categoryResponse = await client.GetAsync("https://localhost:7126/api/Category");
                if (categoryResponse.IsSuccessStatusCode)
                {
                    string catagoryData = await categoryResponse.Content.ReadAsStringAsync();
                    var category = JsonSerializer.Deserialize<List<Category>>(catagoryData, option);
                    Hotel.Category = category.FirstOrDefault(c => c.CategoryId == Hotel.CategoryId);
                }
            }
            else if (hotelResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            var response = await client.GetAsync($"https://localhost:7126/api/Room/hotel/{hotelId}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                RoomVM = JsonSerializer.Deserialize<List<RoomVM>>(data, option);

                // Goi API lay list image cua phong
                var imagesResponse = await client.GetAsync($"https://localhost:7126/api/RoomImage/images/{hotelId}");
                if (imagesResponse.IsSuccessStatusCode)
                {
                    string imagesData = await imagesResponse.Content.ReadAsStringAsync();
                    RoomImages = JsonSerializer.Deserialize<List<RoomImages>>(imagesData, option);
                    foreach(var room in RoomVM)
                    {
                        var roomImg = RoomImages.FirstOrDefault(img => img.RoomId == room.RoomId);
                        room.RoomImage = roomImg?.ImageUrl;
                    }
                }

                // Lấy danh sách đánh giá
                var ratesResponse = await client.GetAsync($"https://localhost:7126/api/Rate/Room/{hotelId}");
                if (ratesResponse.IsSuccessStatusCode)
                {
                    string ratesData = await ratesResponse.Content.ReadAsStringAsync();
                    Rates = JsonSerializer.Deserialize<List<Rate>>(ratesData, option);
                }

                return Page();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
