using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Partner
{
    public class PartnershipModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PartnershipModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public UserProfile? UserProfile { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            // set mặc định giá trị trước khi đăng kí để trở thành Partner
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/User/GetUserProfile");
            var client = _httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var userProfile = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(userProfile))
                {
                    UserProfile = JsonConvert.DeserializeObject<UserProfile>(userProfile);
                }
                return Page();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return RedirectToPage(error, new { Url = "/Partner/Partnership" });
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/RequestBecamePartner");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return RedirectToPage(error);
            }

        }
    }
}
