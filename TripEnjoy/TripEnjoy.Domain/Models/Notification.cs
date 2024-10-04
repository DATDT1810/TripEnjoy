using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Domain.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        public string NotificationContent { get; set; }
        public DateTime NotificationDate { get; set; }
         
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
