using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;

namespace TripEnjoy.Application.Interface.Payment
{
    public interface IVnPayServices
	{
        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestDTO vnPaymentRequest);
        public VnPaymentResponseDTO PaymentExcute(IQueryCollection collections);
    }
}
