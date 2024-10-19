using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripEnjoy.Application.Interface.Hotel;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Presentation.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HotelController : ControllerBase
	{
		private readonly IHotelService _hotelService;
		private readonly ApplicationDbContext _context;
		public HotelController(IHotelService hotelService, ApplicationDbContext applicationDbContext)
		{
			this._hotelService = hotelService;
			_context = applicationDbContext;
		}
		//check
		[HttpGet]
		public async Task<IActionResult> GetHotels()
		{
			var HotelList = await this._hotelService.HotelsAsync();
			return Ok(HotelList);
		}
		//check
		[HttpGet("{id}", Name = "GetHotel")]
		public async Task<IActionResult> GetHotelById(int id)
		{
			Hotel hotel = await this._hotelService.GetHotelsByIdAsync(id);
			if (hotel == null)
			{
				return NotFound();
			}
			return Ok(hotel);
		}
		//check
		[HttpPost]
		public async Task<IActionResult> AddHotel(Hotel hotel)
		{
			if (hotel == null)
			{
				return BadRequest("Hotel can be null");
			}
			await this._hotelService.AddHotelAsync(hotel);
			return CreatedAtRoute("GetHotel", new { id = hotel.HotelId }, hotel);
		}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, Hotel hotel)
        {
            if (hotel == null)
            {
                return BadRequest("Hotel can be null");
            }
            Hotel obj = await this._hotelService.GetHotelsByIdAsync(id);
            if (obj == null)
            {
                return NotFound("could not be found");
            }
            await this._hotelService.UpdateHotelAsync(hotel);
            return CreatedAtRoute("GetHotel", new { id = hotel.HotelId }, hotel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            Hotel obj = await this._hotelService.GetHotelsByIdAsync(id);
            if (obj == null)
            {
                return BadRequest("Hotel can not null");
            }
            await this._hotelService.DeleteHotelAsync(obj);
            return Ok(obj);
        }

        [HttpGet("user/{Id}", Name = "GetListHotelByUsername")]
        public async Task<IActionResult> GetListHotelByUsername(int Id)
        {
            IEnumerable<Hotel> hotelList = await this._hotelService.GetHotelsByUsernameAsync(Id);
            if (hotelList == null)
            {
                return NotFound();
            }
            return Ok(hotelList);
        }
    }
}
