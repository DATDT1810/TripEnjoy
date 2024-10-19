using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.RoomType;

namespace TripEnjoy.Application.Services.RoomType
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;
        public RoomTypeService(IRoomTypeRepository roomTypeRepository)
        {
            _roomTypeRepository = roomTypeRepository;
        }

        public async Task<IEnumerable<Domain.Models.RoomType>> GetAllRoomTypeAsync()
        {
            return await _roomTypeRepository.GetAllRoomTypeAsync();
        }

        public async Task<Domain.Models.RoomType> GetRoomTypeByIdAsync(int roomTypeId)
        {
            return await _roomTypeRepository.GetRoomTypeByIdAsync(roomTypeId);
        }

        public async Task<Domain.Models.RoomType> CreateRoomTypeAsync(Domain.Models.RoomType roomType)
        {
            return await _roomTypeRepository.CreateRoomTypeAsync(roomType);
        }

        public async Task<Domain.Models.RoomType> DeleteRoomTypeAsync(int roomTypeId)
        {
            return await _roomTypeRepository.DeleteRoomTypeAsync(roomTypeId);
        }

        public async Task<Domain.Models.RoomType> UpdateRoomTypeAsync(int roomTypeId, Domain.Models.RoomType roomType)
        {
            return await _roomTypeRepository.UpdateRoomTypeAsync(roomTypeId, roomType);
        }
    }
}
