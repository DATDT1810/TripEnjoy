using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Payment;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
	public class PaymentRepository : IPaymentRepository
	{
		private readonly ApplicationDbContext _context;
		public PaymentRepository(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task CreatePayment(Booking booking)
		{
			if(booking == null)
			{
				throw new ArgumentNullException(nameof(booking));
			}
			// Create payment
			var payment = new Payment
			{
				BookingId = booking.BookingId,
				PaymentDate = DateTime.Now,
				PaymentStatus = "Success",
				PaymentAmount = booking.BookingTotalPrice,
				AccountId = booking.AccountId
			};
			_context.Payments.Add(payment);
			await _context.SaveChangesAsync();
		}
	}
}
