using Microsoft.AspNetCore.Mvc;
using TripEnjoy.Application.Interface.SearchHotel;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchHotelController : ControllerBase
    {
        private readonly ISearchHotelService _searchHotelService;
        private readonly ApplicationDbContext _context;
        public SearchHotelController(ISearchHotelService searchHotelService, ApplicationDbContext applicationDbContext)
        {
            this._searchHotelService = searchHotelService;
            _context = applicationDbContext;
        }

        [HttpGet("GetByCategoryName")]
        public async Task<IActionResult> GetListHotelByCategoryName(int CategoryId, string HotelAddress)
        {
            IEnumerable<Hotel> listSearchHotelByCategoryName = await this._searchHotelService.GetHotelsByCategoryNameAsync(CategoryId, HotelAddress);
            if (listSearchHotelByCategoryName == null)
            {
                return NotFound();
            }
            return Ok(listSearchHotelByCategoryName);
        }
        [HttpGet("GetByRoomTypeName")]
        public async Task<IActionResult> GetListHoteByRoomTypeName(string RoomTypeName)
        {
            IEnumerable<Hotel> listSearchHotelByRoomTypeName = await this._searchHotelService.GetHotelsByRoomTypeNameAsync(RoomTypeName);
            if (listSearchHotelByRoomTypeName == null)
            {
                return NotFound();
            }
            return Ok(listSearchHotelByRoomTypeName);
        }
        [HttpGet("GetByNameOrAddress")]
        public async Task<IActionResult> GetHotelsByNameOrAddress([FromQuery] string keyword)
        {
            var listSearchHotelByNameOrAddress = await _searchHotelService.GetHotelsByNameOrAddressAsync(keyword);
            if (listSearchHotelByNameOrAddress == null || !listSearchHotelByNameOrAddress.Any())
            {
                return NotFound();
            }
            return Ok(listSearchHotelByNameOrAddress);
        }
        [HttpGet("GetAvailableHotels")]
        public async Task<IActionResult> GetAvailableHotels(int categoryId, string hotelAddress, string roomTypeName, DateTime checkinDate, DateTime checkoutDate, int roomQuantity)
        {
            IEnumerable<Hotel> listSearchAvailableHotels = await this._searchHotelService.GetAvailableHotelsAsync(categoryId, hotelAddress, roomTypeName, checkinDate, checkoutDate, roomQuantity);
            if (listSearchAvailableHotels == null)
            {
                return NotFound();
            }
            return Ok(listSearchAvailableHotels);
        }
        [HttpGet("GetCommentsByRoomId")]
        public async Task<IActionResult> GetCommentsByRoomId(int roomId)
        {
            IEnumerable<Comment> listCommentsByRoomId = await this._searchHotelService.GetCommentsByRoomIdAsync(roomId);
            if (listCommentsByRoomId == null)
            {
                return NotFound();
            }
            return Ok(listCommentsByRoomId);
        }

    }
}
