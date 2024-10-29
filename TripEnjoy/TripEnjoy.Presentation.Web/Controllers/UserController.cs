using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.ImageCloud;
using TripEnjoy.Application.Interface.User;
using TripEnjoy.Application.Services.ImageCloud;
using TripEnjoy.Infrastructure.Service;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserServices userServices;
        private readonly ImageManagementServices imageManagementServices;

        public UserController(IUserServices userServices, ImageManagementServices imageManagementServices)
        {
            this.userServices = userServices;
            this.imageManagementServices = imageManagementServices;
        }

        [Authorize]
        [HttpGet]
        [Route("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
             var userId = User.FindFirstValue(ClaimTypes.Email);
           // var userId = "haodangtest1@gmail.com"; // for testing
            if (userId != null)
            { 
                var userProfile = await userServices.GetUserProfile(userId);
                if (userProfile != null)
                {
                    return Ok(userProfile);
                }
            }
            return NotFound("User not found");
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfile userProfile)
        {      
                if (userProfile != null)
                {               
                    var updatedUserProfile = await userServices.UpdateUserProfile(userProfile);
                    if (updatedUserProfile != null)
                    {
                        return Ok(updatedUserProfile);
                    }
                }          
            return BadRequest("Invalid data");
        }

        //[Authorize]
        [HttpPost]
        [Route("UploadImageProfile")]
        public async Task<IActionResult> UploadImageProfile(List<IFormFile> files)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return Unauthorized("Invalid token");
            }
            if (files == null || files.Count == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var fileIds = new List<string>();
            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    return BadRequest("File is empty.");
                }
                using (var stream = file.OpenReadStream())
                {

                    var fileId = await imageManagementServices.UploadImage(file, email);
                    fileIds.Add(fileId);
                }
            }
            return Ok(new { FileIds = fileIds });
        }
    }
}
