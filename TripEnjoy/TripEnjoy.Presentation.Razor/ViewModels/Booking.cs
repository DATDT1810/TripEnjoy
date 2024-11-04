using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class Booking
    {
        public int BookingId { get; set; } 
        public int AccountId { get; set; } 
        public Account Account { get; set; }
        public DateTime CheckinDate { get; set; }  
        public DateTime CheckoutDate { get; set; } 
        public string BookingStatus { get; set; }
        public int RoomQuantity { get; set; } 
        public int RoomId { get; set; }
        public ViewModels.Room Room { get; set; }
        public int VoucherId { get; set; } 
        public decimal BookingTotalPrice { get; set; }
        public VoucherVM VoucherVM { get; set; }
    }
}
