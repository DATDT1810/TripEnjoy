namespace TripEnjoy.Presentation.Razor.ViewModels
{

    // Sử dụng cho việc gán dữ liệu Value rate và Comment và Reply của phòng
    public class RoomReview
    {
        public int CommentId { get; set; }
        public int AccountId { get; set; }
        public int RoomId { get; set; }
        public string CommentContent { get; set; }
        public int CommentLevel { get; set; }
        public string CommentReply { get; set; }
        public DateTime CommentDate { get; set; }
        public string AccountName { get; set; }
        public string AccountImage { get; set; }
        public int? RateId { get; set; } 
        public int? RateValue { get; set; }
    }
}
