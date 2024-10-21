using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.ImageHotel;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
	public class ImageHotelRepository : IImageHotelRepository
	{
		private readonly ApplicationDbContext _context;

        public ImageHotelRepository(ApplicationDbContext applicationDbContext)
        {
            this._context = applicationDbContext;
        }
        public async Task<ImageHotel> AddImageHotelAsync(ImageHotel imageHotel)
		{
			await this._context.ImageHotels.AddAsync(new ImageHotel(imageHotel.ImageUrl, imageHotel.HotelId));
			await this._context.SaveChangesAsync();
			return imageHotel;
		}

		public async Task<ImageHotel> DeleteImageHotelAsync(ImageHotel imageHotel)
		{
			this._context.ImageHotels.Remove(imageHotel);
			await this._context.SaveChangesAsync();
			return imageHotel;
		}

		public async Task<IEnumerable<TripEnjoy.Domain.Models.ImageHotel>> GetImageHotelsByHotelIdAsync(int hotelId)
		{
			return await this._context.ImageHotels.Where(p => p.HotelId == hotelId).ToListAsync();
		}

		public Task<ImageHotel> GetImageHotelsByIdAsync(int imageHotelId)
		{
			return this._context.ImageHotels.FirstOrDefaultAsync(p => p.ImageId == imageHotelId);
		}

		public async Task<IEnumerable<ImageHotel>> ImageHotelsAsync()
		{
			return await this._context.ImageHotels.ToListAsync();
		}

		public async Task<ImageHotel> UpdateImageHotelAsync(ImageHotelViewModel imageHotel)
		{
			if (imageHotel == null)
			{
				return null;
			}
			ImageHotel obj = await this._context.ImageHotels.FirstOrDefaultAsync(p => p.ImageId == imageHotel.ImageId);
			if (obj == null)
			{
				return null;
			}
			obj.ImageUrl = imageHotel.ImageUrl;
			obj.HotelId = imageHotel.HotelId;
			await this._context.SaveChangesAsync();
			return obj;
		}
	}
}
