using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.RoomImage;

namespace TripEnjoy.Application.Services.RoomImage
{
    public class RoomImageService : IRoomImageService
    {
        private readonly IRoomImageRepository _roomImageRepository;

        public RoomImageService(IRoomImageRepository roomImageRepository)
        {
            _roomImageRepository = roomImageRepository;
        }

        public async Task<IEnumerable<Domain.Models.RoomImage>> GetAllRoomImagesAsync()
        {
            return await _roomImageRepository.GetAllRoomImagesAsync();
        }

        public async Task<Domain.Models.RoomImage> GetRoomImageByIdAsync(int roomImgId)
        {
            return await _roomImageRepository.GetRoomImageByIdAsync(roomImgId);
        }

        public async Task<IEnumerable<Domain.Models.RoomImage>> GetRoomImagesByRoomIdAsync(int roomId)
        {
            return await _roomImageRepository.GetRoomImagesByRoomIdAsync(roomId);
        }
    }
}
