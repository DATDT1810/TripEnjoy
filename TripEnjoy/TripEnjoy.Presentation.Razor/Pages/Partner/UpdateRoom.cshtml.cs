using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Partner
{
    public class UpdateRoomModel : PageModel
    {

        private readonly IHttpClientFactory httpClientFactory;

        public UpdateRoomModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public RoomVM Room { get; set; }

 

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == 0)
            {
                return RedirectToPage("/Partner/Dashboard");
            }
            var client = httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Room/" + id);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Room = JsonConvert.DeserializeObject<RoomVM>(content) ?? new RoomVM();
                return Page();
            }
            else
            {
                var redirectPath = await response.Content.ReadAsStringAsync();
                return RedirectToPage(redirectPath, new { Url = "/Partner/UpdateRoom" });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var client = httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7126/api/Room/" + Room.RoomId);
            request.Content = new StringContent(JsonConvert.SerializeObject(Room), Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Partner/RoomDashboard");
            }
            return RedirectToPage();
        }
    }
}
