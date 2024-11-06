using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
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
    public class ImageService : IImageService
    {

        private readonly Cloudinary cloudinary;

        public ImageService(IOptions<CloudinarySetting> config)
        {
            var acc = new Account
                (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> UploadImage(IFormFile file, string email)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = $"user/{email}"
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }



        public Task<DeletionResult> DeleteImage(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = cloudinary.DestroyAsync(deleteParams);
            return result;
        }
    }
}
