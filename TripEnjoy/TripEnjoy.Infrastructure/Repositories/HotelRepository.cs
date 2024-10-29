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
            hotel.IsDeleted = true;
            await this._context.Hotels.AddAsync(new Hotel(hotel.HotelId, hotel.HotelName, hotel.HotelAddress, hotel.HotelPhone, hotel.HotelDescription, hotel.IsDeleted, hotel.HotelStatus, hotel.HotelTimeStart, hotel.HotelTimeEnd, hotel.AccountId, hotel.CategoryId));
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
            Hotel? hotel1 = await this._context.Hotels.FirstOrDefaultAsync(p => p.HotelId == hotel.HotelId);
            if (hotel1 == null)
            {
                return null;
            }
            hotel1.HotelName = hotel.HotelName;
            hotel1.HotelAddress = hotel.HotelAddress;
            hotel1.HotelPhone = hotel.HotelPhone;
            hotel1.HotelDescription = hotel.HotelDescription;
            hotel1.IsDeleted = hotel1.IsDeleted;
            hotel1.HotelStatus = hotel.HotelStatus;
            hotel1.HotelTimeStart = hotel.HotelTimeStart;
            hotel1.HotelTimeEnd = hotel.HotelTimeEnd;
            hotel1.AccountId = hotel.AccountId;
            hotel1.CategoryId = hotel.CategoryId;
            await this._context.SaveChangesAsync();
            return hotel;
        }
    }
}
