using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.RoomImage
{
    public interface IRoomImageService
    {
        Task<IEnumerable<Domain.Models.RoomImage>> GetAllRoomImagesAsync();
        Task<Domain.Models.RoomImage> GetRoomImageByIdAsync(int roomImgId);
        Task<IEnumerable<Domain.Models.RoomImage>> GetRoomImagesByRoomIdAsync(int roomId); 
    }
}
