using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.WebSockets;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Partner
{
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<BookingOfPartner> Bookings { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var client = httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var bookingRequest = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Booking/GetBookingByPartner");
            var bookingResponse = await client.SendAsync(bookingRequest);
            if (bookingResponse.IsSuccessStatusCode)
            {
                var bookingData = await bookingResponse.Content.ReadAsStringAsync();
                Bookings = JsonConvert.DeserializeObject<List<BookingOfPartner>>(bookingData);
                return Page();
            }
            else
            {
                var redirectPath = await bookingResponse.Content.ReadAsStringAsync();
                return RedirectToPage(redirectPath);
            }
        }
    }
}
