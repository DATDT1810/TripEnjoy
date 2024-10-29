using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.SearchHotel;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Services.SearchHotel
{
    public class SearchHotelService : ISearchHotelService
    {
        private readonly ISearchHotelRepository _searchHotelRepository;
        public SearchHotelService(ISearchHotelRepository searchHotelRepository)
        {
            this._searchHotelRepository = searchHotelRepository;
        }

        public async Task<IEnumerable<Hotel>> GetHotelsByCategoryNameAsync(int CategoryId, string HotelAddress) => await this._searchHotelRepository.GetHotelsByCategoryNameAsync(CategoryId, HotelAddress);
        public async Task<IEnumerable<Hotel>> GetHotelsByRoomTypeNameAsync(string RoomTypeName) => await this._searchHotelRepository.GetHotelsByRoomTypeNameAsync(RoomTypeName);
        public async Task<IEnumerable<Hotel>> GetHotelsByNameOrAddressAsync(string KeyWord) => await this._searchHotelRepository.GetHotelsByNameOrAddressAsync(KeyWord);
        public async Task<IEnumerable<Hotel>> GetAvailableHotelsAsync(int categoryId, string hotelAddress, string roomTypeName, DateTime checkinDate, DateTime checkoutDate, int roomQuantity) => await this._searchHotelRepository.GetAvailableHotelsAsync(categoryId, hotelAddress, roomTypeName, checkinDate, checkoutDate, roomQuantity);
        public async Task<IEnumerable<TripEnjoy.Domain.Models.Comment>> GetCommentsByRoomIdAsync(int roomId) => await this._searchHotelRepository.GetCommentsByRoomIdAsync(roomId);

    }
}
