using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Models
{
    public class ImageHotel
    {
        [Key]
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }

        public int HotelId { get; set; }
        [ForeignKey("HotelId")]
        public virtual Hotel? Hotel { get; set; }

        public ImageHotel()
        {
            
        }

		public ImageHotel(int imageId, string imageUrl, int hotelId)
		{
			ImageId = imageId;
			ImageUrl = imageUrl;
			HotelId = hotelId;
		}

		public ImageHotel(string imageUrl, int hotelId)
		{
			ImageUrl = imageUrl;
			HotelId = hotelId;
		}
	}

	public class ImageHotelViewModel
	{
		public int ImageId { get; set; }
		public string ImageUrl { get; set; }

		public int HotelId { get; set; }
		public ImageHotelViewModel()
		{

		}

		public ImageHotelViewModel(int imageId, string imageUrl, int hotelId)
		{
			ImageId = imageId;
			ImageUrl = imageUrl;
			HotelId = hotelId;
		}

		public ImageHotelViewModel(string imageUrl, int hotelId)
		{
			ImageUrl = imageUrl;
			HotelId = hotelId;
		}
	}
}
