using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Hotel;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Services
{
	public class HotelService : IHotelService
	{
		private readonly IHotelRepository _hotelRepository;
        public HotelService(IHotelRepository hotelRepository)
        {
            this._hotelRepository = hotelRepository;
        }

		public async Task<Hotel> AddHotelAsync(Hotel hotel) => await this._hotelRepository.AddHotelAsync(hotel);

		public async Task<Hotel> DeleteHotelAsync(Hotel hotel) => await this._hotelRepository.DeleteHotelAsync(hotel);

		public async Task<Hotel> GetHotelsByIdAsync(int Id) => await this._hotelRepository.GetHotelsByIdAsync(Id);

		public async Task<IEnumerable<Hotel>> GetHotelsByUsernameAsync(int AccountId) => await this._hotelRepository.GetHotelsByUsernameAsync(AccountId);

		public async Task<IEnumerable<Hotel>> HotelsAsync() => await this._hotelRepository.HotelsAsync();

		public async Task<Hotel> UpdateHotelAsync(Hotel hotel) => await this._hotelRepository.UpdateHotelAsync(hotel);
	}
}
