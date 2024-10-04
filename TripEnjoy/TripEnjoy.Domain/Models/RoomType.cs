using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Domain.Models
{
    public class RoomType
    {
        [Key]
        [Required]
        public int RoomTypeId { get; set; }
       
        [Required]
        [StringLength(50)]
        public string RoomTypeName { get; set; }
        public bool RoomTypeStatus { get; set; }
    }
}
