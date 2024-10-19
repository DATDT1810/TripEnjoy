using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;

namespace TripEnjoy.Presentation.Razor.Pages.Payment
{
    public class PaymentModel : PageModel
    {
        private readonly IHttpClientFactory httpClient;
        public PaymentModel(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient;
        }
        public int BookingId { get; set; }
        public async Task<IActionResult> OnGet()
        {
            BookingId = Convert.ToInt32(Request.Query["bookingId"]);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Payment/CreatePaymentUrl");
            request.Content = new StringContent(JsonConvert.SerializeObject(BookingId), Encoding.UTF8, "application/json");
            var client = httpClient.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var paymentResponse = await response.Content.ReadAsStringAsync();
                var paymentUrl = JsonConvert.DeserializeObject<dynamic>(paymentResponse)?.paymentUrl;
                if (paymentUrl != null)
                {
                    return Redirect(paymentUrl.ToString());
                }
            }
            return RedirectToPage("/Home/404");
        }

    }
}
