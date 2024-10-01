using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Pages
{
    public class RegisterModel : PageModel
    {

        private readonly IHttpClientFactory _clientFactory;
        public RegisterModel(IHttpClientFactory clientFactory)
        {
            this._clientFactory = clientFactory;

        }
        [BindProperty(SupportsGet = true)]
        public RegisterViewModel RegisterViewModel { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var model = new
                {
                    email = registerViewModel.email,
                    password = registerViewModel.password
                };
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/Register");
                request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var client = _clientFactory.CreateClient();
                try
                {
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("/Login");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid register attempt.");
                        return Page();
                    }
                }
                catch (HttpRequestException e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    return Page();
                }
            }
            return Page();
        }
    }
}
