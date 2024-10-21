using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.HotelImage;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Services.HotelImage
{
    public class HotelImageService : IHotelImageService
    {
        private readonly IHotelImageRepository _hotelImageRepository;

        public HotelImageService(IHotelImageRepository hotelImageRepository)
        {
            _hotelImageRepository = hotelImageRepository;
        }

        public async Task<IEnumerable<Domain.Models.ImageHotel>> GetAllHotelImagesAsync()
        {
            return await _hotelImageRepository.GetAllHotelImagesAsync();
        }

        public async Task<Domain.Models.ImageHotel> GetHotelImageByIdAsync(int hotelImgId)
        {
            return await _hotelImageRepository.GetHotelImageByIdAsync(hotelImgId);

        }
    }
}
