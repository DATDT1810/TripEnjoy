using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class RoomReviewDTO
    {
        // Comment Account Info
        public int RoomId { get; set; }
        public int AccountId { get; set; }
        public int RateValue { get; set; } 
        public int CommentId { get; set; }
        public int CommentLevel { get; set; }   
        public string? CommentContent { get; set; }
        public DateTime ReviewDate { get; set; }
        public string? FullName { get; set; }
        public string? Image { get; set; }

        // Reply Comment Account Info
        public int? AccountIdReply { get; set; }
        public string? AccountNameReply { get; set; }
        public DateTime? ReplyDate { get; set; }
        public int? CommentIdReply { get; set; }
      //  public int? CommentLevelReply { get; set; }
        public string? CommentReplyContent { get; set; }
        public string? ImageReply { get; set; }

        // Check if the comment is a reply
        public bool IsReply { get; set; } =  false;
    }
}
