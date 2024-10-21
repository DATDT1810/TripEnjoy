using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.HotelImage;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class HotelImageRepository : IHotelImageRepository
    {
        private readonly ApplicationDbContext _context;

        public HotelImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Models.ImageHotel>> GetAllHotelImagesAsync()
        {
            return await _context.ImageHotels.ToListAsync();
        }
        

        public async Task<Domain.Models.ImageHotel> GetHotelImageByIdAsync(int hotelImgId)
        {
            var hotelImage = await _context.ImageHotels.FindAsync(hotelImgId);
            if (hotelImage == null)
            {
                throw new KeyNotFoundException($"HotelImage with ID {hotelImgId} not found.");
            }
            return hotelImage;
        }
    }
}
