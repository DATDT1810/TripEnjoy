
using System.Text.Json.Serialization;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string HotelAddress { get; set; }
        public string HotelPhone { get; set; }
        public string HotelDescription { get; set; }
        public bool IsDeleted { get; set; }
        public string HotelStatus { get; set; }
        public DateTime HotelTimeStart { get; set; }
        public DateTime HotelTimeEnd { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [JsonPropertyName("rooms")]
        public List<RoomVM> Rooms { get; set; } = new List<RoomVM>();
        [JsonPropertyName("imageHotels")]
        public List<HotelImages> HotelImages { get; set; } = new List<HotelImages>();
    }
}
