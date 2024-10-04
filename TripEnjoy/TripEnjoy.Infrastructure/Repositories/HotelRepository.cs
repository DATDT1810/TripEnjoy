using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Hotel;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
	public class HotelRepository : IHotelRepository
	{
		private readonly ApplicationDbContext _context;

        public HotelRepository(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }

		public async Task<Hotel> AddHotelAsync(Hotel hotel)
		{
			await this._context.AddAsync(hotel);
			await this._context.SaveChangesAsync();
			return hotel;
		}

		public async Task<Hotel> DeleteHotelAsync(Hotel hotel)
		{
			this._context.Hotels.Remove(hotel);
			await this._context.SaveChangesAsync();
			return hotel;
		}

		public async Task<Hotel> GetHotelsByIdAsync(int Id) => await this._context.Hotels.FirstOrDefaultAsync(p => p.HotelId.Equals(Id));

		public async Task<IEnumerable<Hotel>> GetHotelsByUsernameAsync(int AccountId) => await this._context.Hotels.Where(p => p.AccountId.Equals(AccountId)).ToListAsync();

		public async Task<IEnumerable<TripEnjoy.Domain.Models.Hotel>> HotelsAsync() => await this._context.Hotels.ToListAsync();

		public async Task<Hotel> UpdateHotelAsync(Hotel hotel)
		{
			_context.Hotels.Update(hotel);
			await _context.SaveChangesAsync();
			return hotel;
		}
	}
}
