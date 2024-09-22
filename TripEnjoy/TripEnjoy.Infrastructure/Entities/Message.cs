using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Infrastructure.Entities
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        [Required]
        public string MessageContent { get; set; }
        [Required]
        public DateTime MessageDate { get; set; }
        public bool MessageStatus { get; set; }

        [ForeignKey("ConversationId")]
        public virtual Conversation Conversation { get; set; }
        [ForeignKey("SenderId")]
        public virtual Account Account { get; set; }
    }
}
