using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages.User
{
    public class ViewProfileModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ViewProfileModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [BindProperty(SupportsGet = true)]
        public UserProfile? UserProfile { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<ViewModels.Booking> Booking { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            var client = httpClientFactory.CreateClient("DefaultClient");
            client.Timeout = TimeSpan.FromMinutes(2);
            // profile
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/User/GetUserProfile");        
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var userProfile = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(userProfile))
                {
                    UserProfile = JsonConvert.DeserializeObject<UserProfile>(userProfile);
                }

                // booking history
                var bookingRequest = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7126/api/Booking");
                var bookingResponse = await client.SendAsync(bookingRequest);
                if (bookingResponse.IsSuccessStatusCode)
                {
                    var bookingContent = await bookingResponse.Content.ReadAsStringAsync();
                    Booking = JsonConvert.DeserializeObject<List<ViewModels.Booking>>(bookingContent);
                }
                return Page();
            }   
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();

                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(UserProfile userProfile)
        {

            if (ModelState.IsValid)
            {            
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/User/UpdateUserProfile");
                request.Content = new StringContent(JsonConvert.SerializeObject(UserProfile), Encoding.UTF8, "application/json");
                var client = httpClientFactory.CreateClient("DefaultClient");
                client.Timeout = TimeSpan.FromMinutes(5);
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    ViewData["message"] = "Update profile successfully";
                }
                else
                {
                    ViewData["message"] = "Update profile failed";
                }
            }
            else
            {
                UserProfile ??= new UserProfile();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUploadImage(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                ViewData["message"] = "No file uploaded.";
                return Page();
            }
            using (var client = httpClientFactory.CreateClient("DefaultClient"))
            {
                client.Timeout = TimeSpan.FromMinutes(2);
                // Tạo đối tượng MultipartFormDataContent để chứa các tệp tin
                using (var form = new MultipartFormDataContent())
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var stream = file.OpenReadStream();
                            var fileContent = new StreamContent(stream);
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                            form.Add(fileContent, "files", file.FileName); // Thêm tệp vào form
                        }
                    }
                 

                    // Gửi yêu cầu POST đến API
                    var response = await client.PostAsync("https://localhost:7126/api/User/UploadImageProfile", form);

                    if (response.IsSuccessStatusCode)
                    {
                        ViewData["message"] = "Image uploaded successfully.";
                    }
                    else
                    {
                        ViewData["message"] = $"Upload failed: {await response.Content.ReadAsStringAsync()}";
                    }
                }
            }
            return RedirectToPage();
        }
    }
}
