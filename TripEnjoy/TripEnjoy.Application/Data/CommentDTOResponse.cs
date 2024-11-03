using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class CommentDTOResponse
    {
        public int CommentId { get; set; }
        public string? Content { get; set; }
        public DateTime CommentDate { get; set; }
        public string? AccountFullname { get; set; }
        public string? AccountImage { get; set; }
    }
}
