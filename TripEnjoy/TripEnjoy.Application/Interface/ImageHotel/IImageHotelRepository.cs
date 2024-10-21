using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.ImageHotel
{
	public interface IImageHotelRepository
	{
		Task<IEnumerable<TripEnjoy.Domain.Models.ImageHotel>> ImageHotelsAsync();
		Task<TripEnjoy.Domain.Models.ImageHotel> GetImageHotelsByIdAsync(int imageHotelId);
		Task<IEnumerable<TripEnjoy.Domain.Models.ImageHotel>> GetImageHotelsByHotelIdAsync(int hotelId);
		Task<TripEnjoy.Domain.Models.ImageHotel> AddImageHotelAsync(TripEnjoy.Domain.Models.ImageHotel imageHotel);
		Task<TripEnjoy.Domain.Models.ImageHotel> UpdateImageHotelAsync(TripEnjoy.Domain.Models.ImageHotelViewModel imageHotel);
		Task<TripEnjoy.Domain.Models.ImageHotel> DeleteImageHotelAsync(TripEnjoy.Domain.Models.ImageHotel imageHotel);
	}
}
