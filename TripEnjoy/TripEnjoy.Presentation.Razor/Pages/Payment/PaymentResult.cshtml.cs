using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
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
			var client = httpClient.CreateClient();
			client.Timeout = TimeSpan.FromMinutes(2);
			var response = await client.SendAsync(request);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();				
				if (result == "Paid")
				{
					return RedirectToPage("/Payment/Success");
				}
				return RedirectToPage("/Payment/Failed");
			}
			return RedirectToPage("/Home/404");
		}
	}
}
