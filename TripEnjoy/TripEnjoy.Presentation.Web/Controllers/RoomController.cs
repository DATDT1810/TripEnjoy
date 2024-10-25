using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.Room;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        // Get all rooms
        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var room = await _roomService.GetAllRoomAsync();
            return Ok(room);
        }

        // Get room details by room ID
        [HttpGet("{roomId}", Name = "GetRoomById")]
        public async Task<IActionResult> GetRoomById(int roomId)
        {
            var room = await _roomService.GetRoomDetailByIdAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        // Get rooms by hotel ID
        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetRoomByHotelId(int hotelId)
        {
            var rooms = await _roomService.GetRoomsByHotelIdAsync(hotelId);
            if (rooms == null) { return NotFound(); }
            return Ok(rooms);
        }

        // Create a new room
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] RoomDTO roomDTO)
        {
            if (roomDTO == null)
            {
                return BadRequest();
            }
            var newRoom = _mapper.Map<Room>(roomDTO);
            await _roomService.CreateRoomAsync(newRoom);
            return CreatedAtRoute("GetRoomById", new { id = newRoom.RoomId }, newRoom);
        }

        //Update an existing room
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDTO roomDTO)
        {
            if(roomDTO == null)
            {
                return BadRequest("Invalid data!!!");
            }
            Room obj = await _roomService.GetRoomDetailByIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            var updateRoom = _mapper.Map<Room>(roomDTO);
            await _roomService.UpdateRoomAsync(id, updateRoom);
            return Ok(obj);
        }

        // Delete a room by room ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            Room obj = await _roomService.GetRoomDetailByIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            await _roomService.DeleteRoomAsync(id);

            return NoContent(); // Return 204 No Content if deletion was successful
        }

        [HttpGet("related/{roomTypeId}/{hotelId}")]
        public async Task<IActionResult> GetRelatedRooms(int roomTypeId, int hotelId)
        {
            var relatedRooms = await _roomService.GetRelatedRoomsByRoomTypeIdAsync(roomTypeId, hotelId);
            if (relatedRooms == null || !relatedRooms.Any())
            {
                return NotFound("No related rooms found.");
            }

            return Ok(relatedRooms);
        }


    }
}
