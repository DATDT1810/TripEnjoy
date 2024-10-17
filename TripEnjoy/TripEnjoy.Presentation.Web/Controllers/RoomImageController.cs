using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripEnjoy.Application.Interface.RoomImage;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomImageController : ControllerBase
    {
        private readonly IRoomImageService _roomImageService;

        public RoomImageController(IRoomImageService roomImageService)
        {
            _roomImageService = roomImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoomImage()
        {
            var roomImageList = await _roomImageService.GetAllRoomImagesAsync();
            return Ok(roomImageList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomImageById(int id)
        {
            var roomImage = await _roomImageService.GetRoomImageByIdAsync(id);
            if (roomImage == null)
            {
                return NotFound();
            }
            return Ok(roomImage);
        }

        // API để lấy tất cả hình ảnh của một phòng theo RoomId
        [HttpGet("images/{roomId}")]
        public async Task<IActionResult> GetRoomImagesByRoomId(int roomId)
        {
            var roomImages = await _roomImageService.GetRoomImagesByRoomIdAsync(roomId);
            if (roomImages == null || !roomImages.Any())
            {
                return NotFound();
            }
            return Ok(roomImages);
        }
    }
}
