namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class CommentReply
    {
        public int RoomId { get; set; }
        public string Content { get; set; }
        public string ReplyToComment { get; set; } // ID of the parent comment being replied to
    }
}
