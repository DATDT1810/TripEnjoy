using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class CommentResponseDTO
    {
        public int RoomId { get; set; }
        public string Content { get; set; }
        public string ReplyToComment { get; set; }
    }
}
