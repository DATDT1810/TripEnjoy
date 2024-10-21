using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.ImageCloud;

namespace TripEnjoy.Infrastructure.Service
{
	public class ImageHotelManagementService : IImageHotelManagementService
	{
		private readonly Cloudinary _cloudinary;
		public ImageHotelManagementService(IOptions<CloudinarySetting> config)
		{
			var acc = new Account
				(
				config.Value.CloudName,
				config.Value.ApiKey,
				config.Value.ApiSecret
				);
			_cloudinary = new Cloudinary(acc);
		}
		public Task<DeletionResult> DeleteHotelImage(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);
			var result = _cloudinary.DestroyAsync(deleteParams);
			return result;
		}

		public async Task<ImageUploadResult> UploadHotelImage(IFormFile file, int hotelId, int count)
		{
			var uploadResult = new ImageUploadResult();
			if (file.Length > 0)
			{
				await using var stream = file.OpenReadStream();
				var uploadParams = new ImageUploadParams
				{
					File = new FileDescription(file.FileName, stream),
					PublicId = $"hotel/{hotelId}/{count}"
				};
				uploadResult = await _cloudinary.UploadAsync(uploadParams);
			}
			return uploadResult;
		}
	}
}
