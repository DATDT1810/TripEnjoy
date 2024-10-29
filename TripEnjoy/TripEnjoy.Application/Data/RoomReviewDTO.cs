using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class RoomReviewDTO
    {
        public int RoomId { get; set; }
        public int AccountId { get; set; }
        public int RateValue { get; set; } 
        public string CommentContent { get; set; }
        public DateTime ReviewDate { get; set; }
        public string FullName { get; set; }
    }
}
