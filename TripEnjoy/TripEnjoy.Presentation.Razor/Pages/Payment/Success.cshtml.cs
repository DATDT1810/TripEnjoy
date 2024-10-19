using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TripEnjoy.Presentation.Razor.Pages.Payment
{
	public class SuccessModel : PageModel
	{
		[BindProperty(SupportsGet = true)]
		public int BookingId { get; set; }

		public IActionResult OnGet()
		{
			BookingId = Convert.ToInt32(Request.Query["bookingId"]);
			return Page();
		}
	}
}
