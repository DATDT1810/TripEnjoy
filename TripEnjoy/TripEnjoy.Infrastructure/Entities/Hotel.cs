using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Infrastructure.Entities
{
    public class Hotel
    {
        [Key]
        public int HotelId { get; set; }           // hotel_id

        [Required]
        [StringLength(255)]                        // hotel_name - nvarchar(255)
        public string HotelName { get; set; }

        [StringLength(255)]                        // hotel_address - nvarchar(255)
        public string HotelAddress { get; set; }

        [StringLength(10)]                         // hotel_phone - nvarchar(10)
        public string HotelPhone { get; set; }

        public string HotelDescription { get; set; }   // hotel_description - nvarchar(MAX)

        public bool IsDeleted { get; set; }        // hotel_isDeleted - bit

        [StringLength(255)]                        // hotel_status - varchar(255)
        public string HotelStatus { get; set; }

        public DateTime HotelTimeStart { get; set; }   // hotel_timeStart - datetime
        public DateTime HotelTimeEnd { get; set; }     // hotel_timeEnd - datetime

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]                    // account_id là khóa ngoại trỏ đến bảng Account
        public virtual Account Account { get; set; }       // Điều này thiết lập quan hệ 1-n với bảng Account
    }
}
