using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.RoomType
{
    public interface IRoomTypeRepository
    {
        Task<IEnumerable<Domain.Models.RoomType>> GetAllRoomTypeAsync();
        Task<Domain.Models.RoomType> GetRoomTypeByIdAsync(int roomTypeId);
        Task<Domain.Models.RoomType> CreateRoomTypeAsync(Domain.Models.RoomType roomType);
        Task<Domain.Models.RoomType> UpdateRoomTypeAsync(int roomTypeId, Domain.Models.RoomType roomType);
        Task<TripEnjoy.Domain.Models.RoomType> DeleteRoomTypeAsync(int roomTypeId);
    }
}
