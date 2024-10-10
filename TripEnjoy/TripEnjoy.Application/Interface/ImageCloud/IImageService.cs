using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.ImageCloud
{
    public interface IImageService
    {
        Task<ImageUploadResult> UploadImage(IFormFile file, string email);
        Task<DeletionResult> DeleteImage(string publicId);
       
    }

}
