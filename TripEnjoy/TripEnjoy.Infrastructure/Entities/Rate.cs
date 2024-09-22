using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Infrastructure.Entities
{
    public class Rate
    {
        [Key]
        [Required]
        public int RateId { get; set; }
        [Required]
        public int RateValue { get; set; }
        public DateTime RateDate { get; set; }
     
        public int RoomId { get; set; }
        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
