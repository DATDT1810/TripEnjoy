using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.Room;
using TripEnjoy.Application.Interface.RoomType;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }

        // Get all RoomType
        [HttpGet]
        public async Task<IActionResult> GetAllRoomType()
        {
            var roomType = await _roomTypeService.GetAllRoomTypeAsync();
            return Ok(roomType);
        }

        // Get room details by RoomType ID
        [HttpGet("{id}", Name = "GetRoomTypeById")]
        public async Task<IActionResult> GetRoomTypeById(int id)
        {
            var roomType = await _roomTypeService.GetRoomTypeByIdAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }
            return Ok(roomType);
        }

        // Create a new roomtype
        [HttpPost]
        public async Task<IActionResult> CreateRoomType([FromBody] RoomType roomType)
        {
            if (roomType == null)
            {
                return BadRequest();
            }
            await _roomTypeService.CreateRoomTypeAsync(roomType);
            return CreatedAtRoute("GetRoomById", new { id = roomType.RoomTypeId }, roomType);
        }

        //Update an existing roomtype
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomType(int id, [FromBody] RoomType roomType)
        {
            if (roomType == null)
            {
                return BadRequest("Invalid data!!!");
            }
            RoomType obj = await _roomTypeService.GetRoomTypeByIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _roomTypeService.UpdateRoomTypeAsync(id, obj);
            return Ok(obj);
        }

        // Delete a room by roomtype ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            RoomType obj = await _roomTypeService.GetRoomTypeByIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _roomTypeService.DeleteRoomTypeAsync(id);

            return NoContent(); // Return 204 No Content if deletion was successful
        }
    }
}
