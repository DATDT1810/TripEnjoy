using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Domain.Models
{
    public class Room
    {
        [Key]
        [Required]
        public int RoomId { get; set; }            // room_id

        public int HotelId { get; set; }           // hotel_id (khóa ngoại đến bảng Hotel)

        [Required]
        [StringLength(255)]                        // room_title - nvarchar(255)
        public string RoomTitle { get; set; }

        public int RoomTypeId { get; set; }          // room_type - int

        public int RoomQuantity { get; set; }      // room_quantity - int

        public int RoomStatusID { get; set; }        // room_status - int

        public decimal RoomPrice { get; set; }     // room_price - money

        public string? RoomDescription { get; set; }  // room_description - nvarchar(MAX)
      
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }
        [ForeignKey("RoomTypeId")]
        public virtual RoomType RoomType { get; set; }     // Điều này thiết lập quan hệ 1-n với bảng RoomType
        [ForeignKey("RoomStatusID")]
        public virtual RoomStatus RoomStatus { get; set; } // Điều này thiết lập quan hệ 1-n với bảng RoomStatus 
           
    }
}
