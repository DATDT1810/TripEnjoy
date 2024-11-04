using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
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
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public BookingController(IBookingService bookingService, IAccountService accountService, IMapper mapper)
        {
            _bookingService = bookingService;
            _accountService = accountService;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBookingList()
        {
            var bookingList = await _bookingService.GetBookingListAsync();
            return Ok(bookingList);
        }

        [HttpGet("{id}", Name = "GetBookingById")]
        [HttpGet("GetBookingId/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [Authorize]
        [HttpPost]
        [Route("CreateBooking")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingDTO bookingDto)
        {
            ClaimsPrincipal claims = this.User;
            var email = claims.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest();
            }
            var account = await _accountService.GetAccountByEmail(email);
            var accountId = account.AccountId;
            if (bookingDto == null)
            {
                return BadRequest();
            }
            bookingDto.AccountId = accountId;
            var booking = _mapper.Map<Booking>(bookingDto);
            
            await _bookingService.CreateBookingAsync(booking);
          return Ok(booking.BookingId);
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

        [HttpGet]
        [Route("CheckBookingStatus")]
        public async Task<IActionResult> CheckBookingStatus([FromBody] int bookingId)
		{
			var booking = await _bookingService.GetBookingByIdAsync(bookingId);
			if (booking == null)
			{
				return NotFound();
			}
			return Ok(booking.BookingStatus);
		}

       // public async Task<IActionResult> CheckRoomBookedForAccount
	}
}
