using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Interface.Booking_Room
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Booking>> GetBookingListAsync();
        Task<Booking> CreateBookingAsync(Booking booking);
        Task<Booking> CancelBookingAsync(int bookingId, int accId);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task UpdateBookingAsync(Booking booking);

        // lấy toàn bộ booking của Phòng này 
        Task<IEnumerable<Booking>> GetBookingByAccoutPartner(string email);
    }
}
