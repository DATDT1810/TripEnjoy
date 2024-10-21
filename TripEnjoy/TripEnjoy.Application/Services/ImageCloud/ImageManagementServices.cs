using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface;
using TripEnjoy.Application.Interface.ImageCloud;

namespace TripEnjoy.Application.Services.ImageCloud
{
    public class ImageManagementServices
    {
        private readonly IImageService imageService;
        private readonly IAccountRepository accountRepository;
        public ImageManagementServices(IImageService imageService, IAccountRepository accountRepository)
        {

            this.imageService = imageService;
            this.accountRepository = accountRepository;
        }
        public async Task<string> UploadImage(IFormFile file, string email)
        {
            var imageResult = await imageService.UploadImage(file, email);
            if (imageResult != null)
            {
                var userProfile = await accountRepository.GetUserProfile(email);
                await imageService.DeleteImage(ExtractImageIdFromUrl(userProfile.AccountImage));
                userProfile.AccountImage = imageResult.SecureUrl.ToString();
                await accountRepository.UpdateUserProfile(userProfile);
            }
            else
            {
                throw new Exception($"User profile not found for email: {email}");
            }
            return imageResult.SecureUrl.ToString();
        }

        public string ExtractImageIdFromUrl(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return null;
            }

            var uploadIndex = imageUrl.IndexOf("/upload/");


            if (uploadIndex == -1)
            {
                return null;
            }

            var idWithVersion = imageUrl.Substring(uploadIndex + "/upload/".Length);

            var idWithoutExtension = idWithVersion.Split(new char[] { '/' })[1];
            var id = idWithoutExtension.Split('.')[0];

            return id;
        }

    }
}
