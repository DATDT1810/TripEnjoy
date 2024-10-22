using System.ComponentModel.DataAnnotations.Schema;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class HotelImages
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public int HotelId { get; set; }
    
    }
}
