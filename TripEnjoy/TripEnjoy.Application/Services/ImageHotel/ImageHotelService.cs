using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.ImageHotel;

namespace TripEnjoy.Application.Services.ImageHotel
{
	public class ImageHotelService : IImageHotelService
	{
		private readonly IImageHotelRepository _imageHotelRepository;
        public ImageHotelService(IImageHotelRepository imageHotelRepository)
        {
            this._imageHotelRepository = imageHotelRepository;
        }
		public async Task<Domain.Models.ImageHotel> AddImageHotelAsync(Domain.Models.ImageHotel imageHotel) => await this._imageHotelRepository.AddImageHotelAsync(imageHotel);

		public async Task<Domain.Models.ImageHotel> DeleteImageHotelAsync(Domain.Models.ImageHotel imageHotel) => await this._imageHotelRepository.DeleteImageHotelAsync(imageHotel);

		public async Task<IEnumerable<Domain.Models.ImageHotel>> GetImageHotelsByHotelIdAsync(int hotelId) => await this._imageHotelRepository.GetImageHotelsByHotelIdAsync(hotelId);

		public async Task<Domain.Models.ImageHotel> GetImageHotelsByIdAsync(int imageHotelId) => await this._imageHotelRepository.GetImageHotelsByIdAsync(imageHotelId);

		public async Task<IEnumerable<Domain.Models.ImageHotel>> ImageHotelsAsync() => await this._imageHotelRepository.ImageHotelsAsync();

		public async Task<Domain.Models.ImageHotel> UpdateImageHotelAsync(Domain.Models.ImageHotelViewModel imageHotel) => await this._imageHotelRepository.UpdateImageHotelAsync(imageHotel);
	}
}
