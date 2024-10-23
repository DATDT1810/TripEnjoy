namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int AccountId { get; set; }
        public int RoomId { get; set; }
        public string CommentContent { get; set; }
        public int CommentLevel { get; set; }
        public string CommentReply { get; set; }
        public DateTime CommentDate { get; set; }
        public string FullName { get; set; }
    }
}
