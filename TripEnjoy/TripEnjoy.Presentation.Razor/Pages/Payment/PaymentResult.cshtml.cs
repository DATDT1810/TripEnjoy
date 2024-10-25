using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace TripEnjoy.Presentation.Razor.Pages.Payment
{
    public class PaymentResultModel : PageModel
    {
        private readonly IHttpClientFactory httpClient;
        public PaymentResultModel(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> OnGet()
        {
            var bookingId = Convert.ToInt32(Request.Query["bookingId"]);
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Booking/CheckBookingStatus");
            request.Content = new StringContent(JsonConvert.SerializeObject(bookingId), Encoding.UTF8, "application/json");
            var client = httpClient.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var response = await client.SendAsync(request);
            var message = "";
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                if (result == "Paid")
                {
                    message = "Payment success!";
                    return RedirectToPage("/Payment/Success", new { message = message });
                }
                message = "Payment failed!";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/Login");
            }
            return RedirectToPage("/Home/404", new { message = message });
        }
    }
}
