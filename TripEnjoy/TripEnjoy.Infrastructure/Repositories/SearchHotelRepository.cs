using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.SearchHotel;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class SearchHotelRepository : ISearchHotelRepository
    {
        private readonly ApplicationDbContext _context;
        public SearchHotelRepository(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }
        public async Task<IEnumerable<Hotel>> GetHotelsByCategoryNameAsync(int CategoryId, string HotelAddress) => await this._context.Hotels.Where(c => c.Category.CategoryId.Equals(CategoryId) && c.HotelAddress.Equals(HotelAddress)).ToListAsync();
        public async Task<IEnumerable<Hotel>> GetHotelsByRoomTypeNameAsync(string RoomTypeName) => await this._context.Hotels.Include(r => r.Rooms).ThenInclude(rt => rt.RoomType).Where(r => r.Rooms.Any(rt => rt.RoomType.RoomTypeName == RoomTypeName)).ToListAsync();
        public async Task<IEnumerable<Hotel>> GetHotelsByNameOrAddressAsync(string Keyword) => await this._context.Hotels.Where(k => k.HotelName.Contains(Keyword) || k.HotelAddress.Contains(Keyword)).ToListAsync();
        public async Task<IEnumerable<Hotel>> GetAvailableHotelsAsync(int categoryId, string hotelAddress, string roomTypeName, DateTime checkinDate, DateTime checkoutDate, int roomQuantity) => await this._context.Hotels.Include(c => c.Category).Include(im => im.ImageHotels).Include(r => r.Rooms).ThenInclude(rt => rt.RoomType).Where(h => h.CategoryId == categoryId && (h.HotelAddress.Contains(hotelAddress) || h.HotelName.Contains(hotelAddress)) && (h.IsDeleted == false) && h.Rooms.Any(rt => rt.RoomType.RoomTypeName == roomTypeName && rt.RoomQuantity >= roomQuantity && !_context.Bookings.Any(b => b.RoomId == rt.RoomId && !(b.CheckoutDate < checkinDate || b.CheckinDate > checkoutDate)))).ToListAsync();
        public async Task<IEnumerable<Comment>> GetCommentsByRoomIdAsync(int roomId)
        {
            return await _context.Comments
                .Where(c => c.RoomId == roomId)
                .ToListAsync();
        }
    }
}
