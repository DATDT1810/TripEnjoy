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
        public IEnumerable<HotelImages> HotelImages { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<Rate> Rates { get; set; }

        [BindProperty(SupportsGet = true)]
        public Hotel Hotel { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int TotalPages { get; set; }

        public RoomListModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int hotelId, int page = 1, int pageSize = 10)
        {
            Page = page; 
            if (hotelId == 0)
            {
                return NotFound();
            }
            
            var client = _httpClientFactory.CreateClient("DefaultClient");

            // fetch hotel
            var hotelResponse = await client.GetAsync($"https://localhost:7126/api/Hotel/{hotelId}");
            if (hotelResponse.IsSuccessStatusCode)
            {
                string hotelData = await hotelResponse.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                Hotel = JsonSerializer.Deserialize<Hotel>(hotelData, option);

                // fetch category   
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
            // Fetch rooms
            var response = await client.GetAsync($"https://localhost:7126/api/Room/hotel/{hotelId}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var allRooms = JsonSerializer.Deserialize<List<RoomVM>>(data, option);

                // Calculate total pages
                TotalPages = (int)Math.Ceiling((double)allRooms.Count / pageSize);

                // Fetch the specific page of rooms
                RoomVM = allRooms.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Fetch room images
                var imagesResponse = await client.GetAsync("https://localhost:7126/api/RoomImage");
                if (imagesResponse.IsSuccessStatusCode)
                {
                    string imagesData = await imagesResponse.Content.ReadAsStringAsync();
                    var allRoomImages = JsonSerializer.Deserialize<List<RoomImages>>(imagesData, option);

                    // Link images to their respective rooms
                    foreach (var room in RoomVM)
                    {
                        room.RoomImages = allRoomImages.Where(img => img.RoomId == room.RoomId).ToList();
                    }
                }

                // Fetch room rates
                var ratesResponse = await client.GetAsync($"https://localhost:7126/api/Rate");
                if (ratesResponse.IsSuccessStatusCode)
                {
                    string ratesData = await ratesResponse.Content.ReadAsStringAsync();
                    Rates = JsonSerializer.Deserialize<List<Rate>>(ratesData, option);
                }

                // Fetch hotel images
                var hotelImagesResponse = await client.GetAsync("https://localhost:7126/api/ImageHotel");
                if (hotelImagesResponse.IsSuccessStatusCode)
                {
                    string hotelImagesData = await hotelImagesResponse.Content.ReadAsStringAsync();
                    var allImg = JsonSerializer.Deserialize<List<HotelImages>>(hotelImagesData, option);
                    HotelImages = allImg.Where(img => img.HotelId == hotelId).ToList();
                        
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
