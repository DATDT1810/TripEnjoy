using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Domain.Models
{
    public class RoomStatus
    {
        [Key]
        [Required]
        public int RoomStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string RoomStatusName { get; set; }
    }
}
