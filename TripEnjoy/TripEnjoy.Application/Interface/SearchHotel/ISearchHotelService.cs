using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.SearchHotel
{
    public interface ISearchHotelService
    {
        Task<IEnumerable<TripEnjoy.Domain.Models.Hotel>> GetHotelsByCategoryNameAsync(int CategoryId, string HotelAddress);

        Task<IEnumerable<TripEnjoy.Domain.Models.Hotel>> GetHotelsByRoomTypeNameAsync(string RoomTypeName);
        Task<IEnumerable<TripEnjoy.Domain.Models.Hotel>> GetHotelsByNameOrAddressAsync(string Keyword);
        Task<IEnumerable<TripEnjoy.Domain.Models.Hotel>> GetAvailableHotelsAsync(int categoryId, string hotelAddress, string roomTypeName, DateTime checkinDate, DateTime checkoutDate, int roomQuantity);
        Task<IEnumerable<TripEnjoy.Domain.Models.Comment>> GetCommentsByRoomIdAsync(int roomId);

    }
}
