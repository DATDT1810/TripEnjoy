using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Interface.Payment
{
	public interface IPaymentService
	{
		Task CreatePayment(Booking booking);
	}
}
