using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.Room
{
    public class RoomDetailModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty(SupportsGet = true)]
        public RoomDetail RoomDetail { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<RoomImages> RoomImages { get; set; }
        public RoomDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7126/api/Room/{id}");
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                RoomDetail = JsonSerializer.Deserialize<RoomDetail>(data, option);

                // Goi API lay list image cua phong
                var imagesResponse = await client.GetAsync($"https://localhost:7126/api/RoomImage/images/{id}");
                if (imagesResponse.IsSuccessStatusCode)
                {
                    string imagesData = await imagesResponse.Content.ReadAsStringAsync();
                    RoomImages = JsonSerializer.Deserialize<List<RoomImages>>(imagesData, option);
                }
                return Page();
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(); 
            }
            
            return Page();
            
        }
    }
}
