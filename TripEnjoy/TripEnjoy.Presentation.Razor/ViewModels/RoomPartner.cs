namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class RoomPartner
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public string? RoomTitle { get; set; }
        public int RoomTypeId { get; set; }          // room_type - int
        public int RoomQuantity { get; set; }      // room_quantity - int
        public int RoomStatusID { get; set; }        // room_status - int
        public decimal RoomPrice { get; set; }     // room_price - money
        public string? RoomDescription { get; set; }
    }
}
