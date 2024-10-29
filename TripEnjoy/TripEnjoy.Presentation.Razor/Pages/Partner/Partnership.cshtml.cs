using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TripEnjoy.Presentation.Razor.Pages.Partner
{
    public class PartnershipModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

		public PartnershipModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public void OnGet()
        {
        }
    }
}
