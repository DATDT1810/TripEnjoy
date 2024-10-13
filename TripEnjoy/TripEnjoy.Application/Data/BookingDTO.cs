using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class BookingDTO
    {
        public int RoomId { get; set; }
        public int AccountId { get; set; }
        public int RoomQuantity { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public decimal BookingTotalPrice { get; set; }
    }
}
