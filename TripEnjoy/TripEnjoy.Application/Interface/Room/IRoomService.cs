using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.Room
{
    public interface IRoomService
    {
        Task<IEnumerable<TripEnjoy.Domain.Models.Room>> GetAllRoomAsync();
        Task<TripEnjoy.Domain.Models.Room> GetRoomDetailByIdAsync(int roomId);
        Task<IEnumerable<TripEnjoy.Domain.Models.Room>> GetRoomsByHotelIdAsync(int hotelId);
        Task<TripEnjoy.Domain.Models.Room> CreateRoomAsync(TripEnjoy.Domain.Models.Room room);
        Task<TripEnjoy.Domain.Models.Room> UpdateRoomAsync(int roomId, TripEnjoy.Domain.Models.Room room);
        Task<TripEnjoy.Domain.Models.Room> DeleteRoomAsync(int roomId);

    }
}
