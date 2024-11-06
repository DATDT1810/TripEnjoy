using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class RoomPartner
    {
        public int RoomId { get; set; }          
        public int HotelId { get; set; }                              
        public string? HotelName { get; set; }
        public string? RoomTitle { get; set; }
        public int RoomTypeId { get; set; }          // room_type - int
        public int RoomQuantity { get; set; }      // room_quantity - int
        public int RoomStatusID { get; set; }        // room_status - int
        public decimal RoomPrice { get; set; }     // room_price - money
        public string? RoomDescription { get; set; }  // room_description - nvarchar(MAX)
   
    }
}
