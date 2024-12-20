﻿namespace TripEnjoy.Presentation.Razor.ViewModels
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
        public string AccountName { get; set; }
        public string AccountImage { get; set; }

        public int? RateValue { get; set; }
    }
}
