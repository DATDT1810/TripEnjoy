using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }         // comment_id - int (Primary Key)
        public int AccountId { get; set; }         // account_id - int (Foreign Key)
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }       // Khóa ngoại trỏ đến bảng Account
        public int RoomId { get; set; }            // room_id - int (Foreign Key)
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }       // Khóa ngoại trỏ đến bảng Room
        public string CommentContent { get; set; } // comment_content - nvarchar(MAX)
        public int CommentLevel { get; set; }      // comment_level - int

        [StringLength(255)]                        // comment_reply - nvarchar(255) (nullable)
        public string CommentReply { get; set; }
        public DateTime CommentDate { get; set; }  // comment_date - datetime
    }
}
