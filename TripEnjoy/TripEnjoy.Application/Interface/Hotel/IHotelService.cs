using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.Hotel
{
	public interface IHotelService
	{
		Task<IEnumerable<TripEnjoy.Domain.Models.Hotel>> HotelsAsync();
		Task<TripEnjoy.Domain.Models.Hotel> GetHotelsByIdAsync(int Id);
		Task<TripEnjoy.Domain.Models.Hotel> AddHotelAsync(TripEnjoy.Domain.Models.Hotel hotel);
		Task<TripEnjoy.Domain.Models.Hotel> UpdateHotelAsync(TripEnjoy.Domain.Models.Hotel hotel);
		Task<TripEnjoy.Domain.Models.Hotel> DeleteHotelAsync(TripEnjoy.Domain.Models.Hotel hotel);
		Task<IEnumerable<TripEnjoy.Domain.Models.Hotel>> GetHotelsByUsernameAsync(int AccountId);
	}
}
