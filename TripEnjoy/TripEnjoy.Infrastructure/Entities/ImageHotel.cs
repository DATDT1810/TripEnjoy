using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Infrastructure.Entities
{
    public class ImageHotel
    {
        [Key]
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }

        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public virtual Hotel Hotel { get; set; }
    }
}
