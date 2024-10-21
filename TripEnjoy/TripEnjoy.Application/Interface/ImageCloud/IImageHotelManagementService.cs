using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.ImageCloud
{
	public interface IImageHotelManagementService
	{
		Task<ImageUploadResult> UploadHotelImage(IFormFile file, int hotelId, int count);
		Task<DeletionResult> DeleteHotelImage(string publicId);
	}
}
