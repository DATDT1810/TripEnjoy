using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Interface.Booking_Room
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetBookingListAsync();
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> CancelBookingAsync (int bookingId, int accId);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task UpdateBookingAsync(Booking booking);

        // Hàm sử dụng cho việc lấy booking của user Phòng này - partner role
        Task<IEnumerable<Domain.Models.Booking>> GetBookingListByPartner(string email);
    }
}
