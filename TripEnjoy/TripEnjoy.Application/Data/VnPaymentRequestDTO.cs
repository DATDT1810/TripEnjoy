using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class VnPaymentRequestDTO
    {
        public int BookingId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
