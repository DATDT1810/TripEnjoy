using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Security.Claims;
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
            if (bookingDto == null)
            {
                return BadRequest();
            }
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

        [Authorize(Roles = "Partner")]
        [HttpGet]
        [Route("GetBookingByPartner")]
        public async Task<IActionResult> GetBookingByPartner()
        {
           var email =  User.FindFirstValue(ClaimTypes.Email);
         //   var email = "haodangtest1@gmail.com";
            if(email == null)
            {
                return Unauthorized();
            }
            var bookingList = await _bookingService.GetBookingListByPartner(email);
            if(bookingList == null)
            {
                return NotFound();
            }
            var bookingResponse = new List<BookingOfPartnerResponse>();
            foreach (var item in bookingList)
            {
               var booking = _mapper.Map<BookingOfPartnerResponse>(item);
                booking.AccountEmail = item.Account.AccountEmail;
                booking.AccountFullName = item.Account.AccountFullname;
                booking.AccountPhoneNumber = item.Account.AccountPhone;
                booking.HotelId = item.Room.Hotel.HotelId;
                booking.HotelName = item.Room.Hotel.HotelName;
                booking.RoomName = item.Room.RoomTitle;            
                bookingResponse.Add(booking);
            }
            return Ok(bookingResponse);
        }
	}
}
