using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Models
{
    public class Booking
    {
        public int BookingId { get; set; } // booking_id - int (Primary Key)
        public int AccountId { get; set; } // account_id - int (Foreign Key)     
        public DateTime CheckinDate { get; set; }  // checkinDate - datetime
        public DateTime CheckoutDate { get; set; } // checkoutDate - datetime
                // booking_status - nvarchar(10)
        public string BookingStatus { get; set; }   
        public int RoomQuantity { get; set; } // so luong phong khach book
        public int RoomId { get; set; } // room_id - int (Foreign Key) 
        public int VoucherId { get; set; } // voucher_id - int (nullable Foreign Key)  
        public decimal BookingTotalPrice { get; set; }
    }
}
