using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class RoomDTO
    {
        public int RoomId { get; set; }          
        public int HotelId { get; set; }           
        public string RoomTitle { get; set; }
        public int RoomTypeId { get; set; }          
        public int RoomQuantity { get; set; }     
        public int RoomStatusID { get; set; }        
        public decimal RoomPrice { get; set; }     
        public string? RoomDescription { get; set; }  
    }
}
