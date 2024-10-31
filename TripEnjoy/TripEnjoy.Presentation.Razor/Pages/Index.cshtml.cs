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
        public IEnumerable<TripEnjoy.Presentation.Razor.ViewModels.HotelImages> HotelImages { get; set; }
        [BindProperty(SupportsGet = true)]
        public IEnumerable<Category> ListCategory { get; set; }
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
            var hotelInfo = Hotels.SelectMany(hotel => new List<string>
            {
                hotel.HotelName,
                hotel.HotelAddress
            }).ToList();
            ViewData["HotelInfo"] = System.Text.Json.JsonSerializer.Serialize(hotelInfo);
            // Call API to get all hotel images
            var imagesResponse = await client.GetAsync("https://localhost:7126/api/HotelImage");
            if (imagesResponse.IsSuccessStatusCode)
            {
                string imagesData = await imagesResponse.Content.ReadAsStringAsync();
                HotelImages = JsonConvert.DeserializeObject<List<HotelImages>>(imagesData);
                foreach (var hotel in Hotels)
                {
                    hotel.HotelImages = HotelImages.Where(img => img.HotelId == hotel.HotelId).ToList();
                }
            }

            // Call API to get the list of categories
            var categoryResponse = await client.GetAsync("https://localhost:7126/api/Category");

            if (categoryResponse.IsSuccessStatusCode)
            {
                string categoryData = await categoryResponse.Content.ReadAsStringAsync();
                ListCategory = JsonConvert.DeserializeObject<List<Category>>(categoryData);

                // Map the category names to the hotels based on CategoryId
                foreach (var hotel in Hotels)
                {
                    hotel.Category = ListCategory.FirstOrDefault(c => c.CategoryId == hotel.CategoryId);
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(int categoryId, string hotelAddress, string dateBooking, string roomtypeInput)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var dateParts = dateBooking.Split(new string[] { " to " }, StringSplitOptions.None);
            var checkinDate = dateParts[0];
            var checkoutDate = dateParts[1];
            Console.WriteLine(checkoutDate);
            var roomtype = roomtypeInput.Split(", ");
            var roomTypeName = roomtype[0];
            var roomTypeQuanlity = roomtype[2];
            var roomTypeNameChild = roomtype[1];
            var roomInfo = $"{roomTypeName}, {roomTypeNameChild}";
            ViewData["RoomInfo"] = roomInfo;
            var roomQuantityRoom = roomTypeQuanlity.Split(" ");
            var roomQuantity = roomQuantityRoom[0];
            var apiUrlHotel = $"https://localhost:7126/api/SearchHotel/GetAvailableHotels?categoryId={categoryId}&hotelAddress={Uri.EscapeDataString(hotelAddress)}&roomTypeName={Uri.EscapeDataString(roomInfo)}&checkinDate={Uri.EscapeDataString(checkinDate)}&checkoutDate={Uri.EscapeDataString(checkoutDate)}&roomQuantity={roomQuantity}";
            var responseListSearchHotel = await client.GetAsync(apiUrlHotel);

            var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            string dataHotels = await responseListSearchHotel.Content.ReadAsStringAsync();

            // Giải mã dữ liệu từ JSON
            if (responseListSearchHotel.IsSuccessStatusCode)
            {
                var hotels = System.Text.Json.JsonSerializer.Deserialize<List<TripEnjoy.Presentation.Razor.Model.Hotel>>(dataHotels, option);
                TempData["ListSearchHotel"] = System.Text.Json.JsonSerializer.Serialize(hotels);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy khách sạn nào phù hợp.");
            }

            return RedirectToPage("ListSearchHotel");
        }
    }
}
