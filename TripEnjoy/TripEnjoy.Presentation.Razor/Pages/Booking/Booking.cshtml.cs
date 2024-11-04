using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Booking
{
    public class BookingModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BookingModel(IHttpClientFactory httpClient)
        {
            _httpClientFactory = httpClient;
        }

        [BindProperty(SupportsGet = true)]
        public Hotel Hotel { get; set; }

        [BindProperty(SupportsGet = true)]
        public RoomDetail RoomDetail { get; set; }

        [BindProperty(SupportsGet = true)]
        public BookingViewModel BookingViewModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public UserProfile UserProfile { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            // Lấy thông tin từ Session và gán vào BookingViewModel
            BookingViewModel = new BookingViewModel
            {
                CheckinDate = DateTime.Parse(HttpContext.Session.GetString("CheckinDate")),
                CheckoutDate = DateTime.Parse(HttpContext.Session.GetString("CheckoutDate")),
                RoomQuantity = HttpContext.Session.GetInt32("RoomQuantity") ?? 1,
                RoomId = id
            };


            // Call API to get the room details
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var roomResponse = await client.GetAsync($"https://localhost:7126/api/Room/{id}");
            if (roomResponse.IsSuccessStatusCode)
            {
                string roomData = await roomResponse.Content.ReadAsStringAsync();
                RoomDetail = JsonConvert.DeserializeObject<RoomDetail>(roomData);

                // Tính toán tổng số ngày ở
                int totalDays = (BookingViewModel.CheckoutDate - BookingViewModel.CheckinDate).Days;

                // Tính tổng tiền dựa trên giá phòng, số lượng phòng và số ngày ở
                BookingViewModel.BookingTotalPrice = RoomDetail.RoomPrice * BookingViewModel.RoomQuantity * totalDays;
            }

            // If RoomDetail exists, use its HotelId to get the correct Hotel
            if (RoomDetail != null)
            {
                var hotelResponse = await client.GetAsync($"https://localhost:7126/api/Hotel/{RoomDetail.HotelId}");
                if (hotelResponse.IsSuccessStatusCode)
                {
                    string hotelData = await hotelResponse.Content.ReadAsStringAsync();
                    Hotel = JsonConvert.DeserializeObject<Hotel>(hotelData);
                }
            }

            // Call API to get account details, assuming API returns a JSON array
            var accountResponse = await client.GetAsync("https://localhost:7126/api/User/GetUserProfile");
            if (accountResponse.IsSuccessStatusCode)
            {
                string accountData = await accountResponse.Content.ReadAsStringAsync();
                var profile  = JsonConvert.DeserializeObject<UserProfile>(accountData);
                UserProfile = profile;
            }
            HttpContext.Session.SetString("BookingViewModel", JsonConvert.SerializeObject(BookingViewModel));
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");

            var bookingViewModelJson = HttpContext.Session.GetString("BookingViewModel");
            if (bookingViewModelJson != null)
            {
                BookingViewModel = JsonConvert.DeserializeObject<BookingViewModel>(bookingViewModelJson);
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Booking/CreateBooking");
            request.Content = new StringContent(JsonConvert.SerializeObject(BookingViewModel), Encoding.UTF8, "application/json");
            client.Timeout = TimeSpan.FromMinutes(3);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var bookingId = await response.Content.ReadAsStringAsync();
                return RedirectToPage("/Payment/Payment", new { bookingId });
            }

            return Page();
        }
    }
}
