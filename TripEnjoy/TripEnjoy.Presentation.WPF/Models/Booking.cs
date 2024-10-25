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
        [Key]
        public int BookingId { get; set; } // booking_id - int (Primary Key)
        public int AccountId { get; set; } // account_id - int (Foreign Key)
        [ForeignKey("AccountId")]
        public Account Account { get; set; }
        public DateTime CheckinDate { get; set; }  // checkinDate - datetime
        public DateTime CheckoutDate { get; set; } // checkoutDate - datetime
        [StringLength(10)]                         // booking_status - nvarchar(10)
        public string BookingStatus { get; set; }
        public int RoomQuantity { get; set; } // so luong phong khach book
        public int RoomId { get; set; } // room_id - int (Foreign Key)
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
        public int VoucherId { get; set; } // voucher_id - int (nullable Foreign Key)
        [ForeignKey("VoucherId")]
        public virtual Voucher Voucher { get; set; }
        public decimal BookingTotalPrice { get; set; }
    }
}
