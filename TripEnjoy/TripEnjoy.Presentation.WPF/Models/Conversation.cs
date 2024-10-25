using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; }
        [Required]        
        public int AccountId1 { get; set; }
        [Required]
        public int AccountId2{ get; set; }
        [Required]
        public DateTime ConversationCreatedDate { get; set; }
        [ForeignKey("AccountId1")]
        public virtual Account Account1 { get; set; }
        [ForeignKey("AccountId2")]
        public virtual Account Account2 { get; set; }
    }
}
