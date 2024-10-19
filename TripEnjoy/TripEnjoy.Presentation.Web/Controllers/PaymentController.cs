using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.Booking_Room;
using TripEnjoy.Application.Interface.Payment;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using TripEnjoy.Application.Services.Email;

namespace TripEnjoy.Presentation.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IVnPayServices _vnPayServices;
		private readonly IBookingService _bookingService;
		private readonly IPaymentService _paymentService;
		public PaymentController(IVnPayServices vnPayServices, IBookingService bookingService, IPaymentService paymentService)
		{
			_vnPayServices = vnPayServices;
			_bookingService = bookingService;
			_paymentService = paymentService;
		}

		[HttpPost]
		[Route("CreatePaymentUrl")]
		public async Task<IActionResult> CreatePaymentUrl([FromBody] int bookingId)
		{
			var booking = await _bookingService.GetBookingByIdAsync(bookingId);
			if (booking == null)
			{
				return NotFound();
			}
			var vnPaymentRequest = new VnPaymentRequestDTO
			{
				BookingId = bookingId,
				Amount = booking.BookingTotalPrice,
				FullName = booking.Account.AccountFullname,
				Description = "Thanh toán ",
				CreatedDate = DateTime.Now
			};
			var paymentUrl = _vnPayServices.CreatePaymentUrl(HttpContext, vnPaymentRequest);
			if (string.IsNullOrEmpty(paymentUrl))
			{
				return BadRequest();
			}
			return Ok(new { paymentUrl });
		}

		[HttpGet]
		[Route("PaymentCallBack")]
		public async Task<IActionResult> PaymentCallBack()
		{
			var message = "";
			var response = _vnPayServices.PaymentExcute(Request.Query);
			if (response == null || response.VnPayResponseCode != "00")
			{
				message = "Payment Error. Please Try Again";
				return Redirect($"https://localhost:7112/Booking/Booking?message={message}");
			}
			// description : Thanh toán cho đơn hàng: 1
			int index = response.OrderDescription.IndexOf(":");
			var bookingId = int.Parse(response.OrderDescription.Substring(index + 1).Trim());
			var booking = await _bookingService.GetBookingByIdAsync(bookingId);
			if (booking == null)
			{
				message = "Payment Error. Please Try Again";
				return Redirect($"https://localhost:7112/Booking/Booking?message={message}");
			}
			booking.BookingStatus = "Paid";
			await _bookingService.UpdateBookingAsync(booking);
			await _paymentService.CreatePayment(booking);
			// send email confirm payment
			return Redirect($"https://localhost:7112/Payment/PaymentResult?bookingId={bookingId}");
		}
	}
}
