using Microsoft.AspNetCore.Mvc;
using TripEnjoy.Application.Interface.HotelImage;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelImageController : Controller
    {
        private readonly IHotelImageService _hotelImageService;
        
        public HotelImageController(IHotelImageService hotelImageService)
        {
            _hotelImageService = hotelImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHotelImages()
        {
            var hotelImageList = await _hotelImageService.GetAllHotelImagesAsync();
            return Ok(hotelImageList);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelImageById(int id)
        {
            var hotelImage = await _hotelImageService.GetHotelImageByIdAsync(id);
            if (hotelImage == null)
            {
                return NotFound();
            }
            return Ok(hotelImage);
        }
       
    }
}
