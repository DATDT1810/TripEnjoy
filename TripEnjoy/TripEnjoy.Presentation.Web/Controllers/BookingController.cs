using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.Booking_Room;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        public BookingController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingList()
        {
            var bookingList = await _bookingService.GetBookingListAsync();
            return Ok(bookingList);
        }

        [HttpGet("{id}", Name = "GetBookingById")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }


        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDto)
        {
            if (bookingDto == null)
            {
                return BadRequest();
            }
            var booking = _mapper.Map<Booking>(bookingDto);
            await _bookingService.CreateBookingAsync(booking);
            return CreatedAtRoute("GetBookingById", new { id = booking.BookingId }, booking);
        }

        [HttpPut("cancel/{accId}")]
        public async Task<IActionResult> CancelBooking(int bookingId, int accId)
        {
            if (accId <= 0)
            {
                return BadRequest("UserId is required to cancel booking.");
            }
            // Gọi service hủy booking
            var cancelBooking = await _bookingService.CancelBookingAsync(bookingId, accId);
            if (cancelBooking == null)
            {
                return NotFound("Booking not found or cannot be cancelled.");
            }
            return Ok(cancelBooking);
        }
    }
}
