using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.HotelImage
{
    public interface IHotelImageService
    {
        Task<IEnumerable<Domain.Models.ImageHotel>> GetAllHotelImagesAsync();
        Task<Domain.Models.ImageHotel> GetHotelImageByIdAsync(int hotelImgId);

    }
}
