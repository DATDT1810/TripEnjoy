using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Data
{
    public class CommentDTO
    {
        public int CommentId { get; set; }     
        public int AccountId { get; set; }        
        public int RoomId { get; set; }           
        public string CommentContent { get; set; } 
        public int CommentLevel { get; set; }      
        public string CommentReply { get; set; } // Id của comment cha
        public DateTime CommentDate { get; set; }
    }
}
