using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class RoomVM
    {
        public int RoomId { get; set; }           
        public int HotelId { get; set; }                           
        public string RoomTitle { get; set; }
        public int RoomTypeId { get; set; }         
        public int RoomQuantity { get; set; }      
        public int RoomStatusID { get; set; }      
        public decimal RoomPrice { get; set; }   
        public string? RoomDescription { get; set; }
        public List<RoomImages> RoomImages { get; set; } = new List<RoomImages>();
    }
}
