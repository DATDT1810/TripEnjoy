namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class BookingViewModel
    {
        public int RoomId { get; set; }
        public int AccountId { get; set; }
        public int RoomQuantity { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public decimal BookingTotalPrice { get; set; }
    }
}
