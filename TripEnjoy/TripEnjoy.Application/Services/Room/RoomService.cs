using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Room;

namespace TripEnjoy.Application.Services.Room
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<Domain.Models.Room>> GetAllRoomAsync()
        {
            return await _roomRepository.GetAllRoomAsync();
        }

        public async Task<Domain.Models.Room> GetRoomDetailByIdAsync(int roomId)
        {
            return await _roomRepository.GetRoomDetailByIdAsync(roomId);
        }

        public async Task<IEnumerable<Domain.Models.Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _roomRepository.GetRoomsByHotelIdAsync(hotelId);
        }


        public async Task<Domain.Models.Room> CreateRoomAsync(Domain.Models.Room room)
        {
            return await _roomRepository.CreateRoomAsync(room);
        }

        public async Task<Domain.Models.Room> DeleteRoomAsync(int roomId)
        {
            return await _roomRepository.DeleteRoomAsync(roomId);
        }
        
        public async Task<Domain.Models.Room> UpdateRoomAsync(int roomId, Domain.Models.Room room)
        {
            return await _roomRepository.UpdateRoomAsync(roomId, room);
        }

        public async Task<IEnumerable<Domain.Models.Room>> GetRelatedRoomsByRoomTypeIdAsync(int roomTypeId, int hotelId)
        {
            return await _roomRepository.GetRelatedRoomsByRoomTypeIdAsync(roomTypeId, hotelId);
        }
    }
}
