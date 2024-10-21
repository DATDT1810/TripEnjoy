using MailKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Hotel;
using TripEnjoy.Application.Interface.ImageCloud;
using TripEnjoy.Application.Interface.ImageHotel;
using TripEnjoy.Application.Services.ImageHotel;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Services.ImageCloud
{
	public class ImageManagementHotelServices
	{
		private readonly IImageHotelManagementService _imageHotelManagementService;
		private readonly IImageHotelService _imageHotelService;

        public ImageManagementHotelServices(IImageHotelManagementService imageHotelManagementService, IImageHotelService imageHotelService)
        {
            this._imageHotelService= imageHotelService;
            this._imageHotelManagementService = imageHotelManagementService;
        }
        public async Task<string> AddImageHotel(IFormFile file, int hotelId, int count)
        {
            var imageResult = await this._imageHotelManagementService.UploadHotelImage(file, hotelId, count);
            if(imageResult != null)
            {
				TripEnjoy.Domain.Models.ImageHotel imageHotel = new Domain.Models.ImageHotel(imageResult.SecureUrl.ToString(), hotelId);
				//imageHotel.ImageUrl = imageResult.SecureUrl.ToString();
				await this._imageHotelService.AddImageHotelAsync(imageHotel);
			}else
            {
				throw new Exception($"Add image Hotel false: {hotelId}");
			}
			return imageResult.SecureUrl.ToString();
		}

	}
}
