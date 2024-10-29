using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.Razor.Model
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
        public virtual Account? Account { get; set; }       // Điều này thiết lập quan hệ 1-n với bảng Account
        public int CategoryId { get; set; }
       
        [ForeignKey("CategoryId")]                   // category_id là khóa ngoại trỏ đến bảng Category
        public virtual Category? Category { get; set; }     // Điều này thiết lập quan hệ 1-n với bảng Category
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
        public virtual ICollection<ImageHotel> ImageHotels { get; set; } = new List<ImageHotel>();
        public Hotel()
        {
            
        }

		public Hotel(int hotelId, string hotelName, string hotelAddress, string hotelPhone, string hotelDescription, bool isDeleted, string hotelStatus, DateTime hotelTimeStart, DateTime hotelTimeEnd, int accountId, int categoryId)
		{
			HotelId = hotelId;
			HotelName = hotelName;
			HotelAddress = hotelAddress;
			HotelPhone = hotelPhone;
			HotelDescription = hotelDescription;
			IsDeleted = isDeleted;
			HotelStatus = hotelStatus;
			HotelTimeStart = hotelTimeStart;
			HotelTimeEnd = hotelTimeEnd;
			AccountId = accountId;
			CategoryId = categoryId;
		}
		public Hotel(string hotelName, string hotelAddress, string hotelPhone, string hotelDescription, bool isDeleted, string hotelStatus, DateTime hotelTimeStart, DateTime hotelTimeEnd, int accountId, int categoryId)
		{
			HotelName = hotelName;
			HotelAddress = hotelAddress;
			HotelPhone = hotelPhone;
			HotelDescription = hotelDescription;
			IsDeleted = isDeleted;
			HotelStatus = hotelStatus;
			HotelTimeStart = hotelTimeStart;
			HotelTimeEnd = hotelTimeEnd;
			AccountId = accountId;
			CategoryId = categoryId;
		}

		public override string? ToString()
		{
			return "Id: " +HotelId + "\r\nHotelName" + this.HotelName +"\r\n address: "+ this.HotelAddress+ "\r\n phone: " + this.HotelPhone+ "\r\n Description: " + this.HotelDescription+ "\r\n IsDeleted: " + this.IsDeleted+ "\r\n Status: " + this.HotelStatus
				+ "\r\n Start: " + this.HotelTimeStart+ "\r\n End: " + this.HotelTimeEnd+ "\r\n Account" + this.AccountId+ "\r\n categoryid" + this.CategoryId + "\r\n account: "+ Account ;
		}
	}
}
