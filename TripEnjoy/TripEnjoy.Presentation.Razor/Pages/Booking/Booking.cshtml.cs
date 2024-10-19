using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Booking
{
    public class BookingModel : PageModel
    {
        private readonly IHttpClientFactory httpClient;
        public BookingModel(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            BookingViewModel bookingViewModel = new BookingViewModel
            {
                RoomId = 2,          
                AccountId =  1,
                RoomQuantity = 2,
                CheckinDate = DateTime.Now,
                CheckoutDate = DateTime.Now.AddDays(3),           
               
    };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Booking/CreateBooking");
            request.Content = new StringContent(JsonConvert.SerializeObject(bookingViewModel), Encoding.UTF8, "application/json");
            var client = httpClient.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var bookingId = await response.Content.ReadAsStringAsync();
                return RedirectToPage("/Payment/Payment",  new {bookingId =  bookingId});
            }
            return Page();
        }
    }
}
