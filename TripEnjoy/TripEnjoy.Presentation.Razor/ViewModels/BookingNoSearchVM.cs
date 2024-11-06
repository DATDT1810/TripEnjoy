namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class BookingNoSearchVM
    {
        public int Quantity { get; set; } = 1;
        public DateTime Checkin { get; set; } =  DateTime.Now;
        public DateTime Checkout { get; set; } = DateTime.Now.AddDays(1);
    }
}
