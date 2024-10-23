using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripEnjoy.Application.Interface.Hotel;
using TripEnjoy.Application.Interface.ImageHotel;
using TripEnjoy.Application.Services.ImageCloud;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;
using TripEnjoy.Infrastructure.Service;

namespace TripEnjoy.Presentation.Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ImageHotelController : ControllerBase
	{
		private readonly IImageHotelService _imageHotelService;
		private readonly ImageManagementHotelServices _imageManagementHotelServices;
		public ImageHotelController(IImageHotelService imageHotelService, ImageManagementHotelServices imageManagementHotelServices)
        {
            this._imageHotelService = imageHotelService;
			this._imageManagementHotelServices = imageManagementHotelServices;
        }
		
		//check
		[HttpGet]
		public async Task<IActionResult> GetAllImageHotel()
		{
			var ImageHotelList = await this._imageHotelService.ImageHotelsAsync();
			return Ok(ImageHotelList);
		}

		[HttpGet("{id}", Name = "GetImageHotelById")]
		public async Task<IActionResult> GetImageHotelById(int id)
		{
			ImageHotel imageHotel = await this._imageHotelService.GetImageHotelsByIdAsync(id);
			if (imageHotel == null)
			{
				return NotFound();
			}
			return Ok(imageHotel);
		}

		[HttpGet("/GetListImageHotelById/{id}", Name = "GetListImageHotelById")]
		public async Task<IActionResult> GetListImageHotelById(int id)
		{
			IEnumerable<ImageHotel> imageHotel = await this._imageHotelService.GetImageHotelsByHotelIdAsync(id);
			if (imageHotel == null)
			{
				return NotFound();
			}
			return Ok(imageHotel);
		}

		[HttpPost]
		[Route("AddImageHotel")]
		public async Task<IActionResult> AddImageHotel(int hotelId,List<IFormFile> files)
		{
			if (hotelId == null)
			{
				return BadRequest("Image hotel data can be null");
			}
			if (files == null || files.Count == 0)
			{
				return BadRequest("No file added.");
			}
			IEnumerable<ImageHotel> ImageHotelList = await this._imageHotelService.ImageHotelsAsync();
			int cout = ImageHotelList.Last().ImageId;
			var fileIds = new List<string>();
			foreach (var file in files)
			{
				if (file.Length == 0)
				{
					return BadRequest("File is empty");
				}
				cout++;
				using (var stream = file.OpenReadStream())
				{
					string fileId = await this._imageManagementHotelServices.AddImageHotel(file, hotelId, cout);
					fileIds.Add(fileId);
				}
			}
			return Ok(new { FileIds = fileIds });
		}

		[HttpPut]
		public async Task<IActionResult> UpdateImageHotel(ImageHotelViewModel imageHotel)
		{
			try
			{
				if (imageHotel == null)
				{
					return BadRequest("imageHotel can be null");
				}
				ImageHotel obj = await this._imageHotelService.GetImageHotelsByIdAsync(imageHotel.ImageId);
				if (obj == null)
				{
					return NotFound("could not be found");
				}
				await this._imageHotelService.UpdateImageHotelAsync(imageHotel);
				//return CreatedAtRoute("GetImageHotelById", new { id = imageHotel.ImageId }, imageHotel);
				return Ok(imageHotel);
			}
			catch (Exception ex)
			{
				// Ghi log lỗi hoặc trả về lỗi chi tiết hơn
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteImageHotel(int id)
		{
			ImageHotel obj = await this._imageHotelService.GetImageHotelsByIdAsync(id);
			if (obj == null)
			{
				return BadRequest("Image hotel can not null");
			}
			await this._imageHotelService.DeleteImageHotelAsync(obj);
			return Ok(obj);
		}
	}
}
