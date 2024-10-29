using System.ComponentModel.DataAnnotations;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class RateAndComment
    {
        public int RoomId { get; set; }
        public int AccountId { get; set; }
        [Range(1, 5, ErrorMessage = "Rate Value must be between 1 and 5.")]
        public int RateValue { get; set; }
        public string CommentContent { get; set; }
        public DateTime ReviewDate { get; set; }
        public int CommentId { get; set; }
    }
}
