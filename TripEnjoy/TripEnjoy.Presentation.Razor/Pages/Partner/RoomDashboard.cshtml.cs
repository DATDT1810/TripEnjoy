using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Partner
{
    public class RoomDashboardModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RoomDashboardModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public IEnumerable<RoomPartner> Rooms { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var client = httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Room/GetRoomsByPartner");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Rooms = JsonConvert.DeserializeObject<IEnumerable<RoomPartner>>(content);
                return Page();
            }
            else
            {
                var redirectPath = await response.Content.ReadAsStringAsync();
                return RedirectToPage(redirectPath, new { Url = "/Partner/RoomDashboard" });
            }
        }

        public async Task<IActionResult> OnPostCreateRoomAsync()
        {
            var client = httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Room/CreateRoom");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Partner/RoomDashboard");
            }
            return Page();
        }
    }
}
