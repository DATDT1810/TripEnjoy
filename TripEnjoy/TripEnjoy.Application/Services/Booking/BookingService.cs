using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.Booking_Room;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Services.Booking
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<TripEnjoy.Domain.Models.Booking>> GetBookingListAsync()
        {
            return await _bookingRepository.GetBookingListAsync();
        }

        public async Task<TripEnjoy.Domain.Models.Booking> CreateBookingAsync(TripEnjoy.Domain.Models.Booking booking)
        {
            return await _bookingRepository.CreateBookingAsync(booking);
        }

        public async Task<TripEnjoy.Domain.Models.Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepository.GetBookingByIdAsync(bookingId);
        }

        public async Task<TripEnjoy.Domain.Models.Booking> CancelBookingAsync(int bookingId, int accId)
        {
            return await _bookingRepository.CancelBookingAsync(bookingId, accId);
        }

        public Task UpdateBookingAsync(Domain.Models.Booking booking)
        {
            _bookingRepository.UpdateBookingAsync(booking);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Domain.Models.Booking>> GetBookingListByPartner(string email)
        {
            return await _bookingRepository.GetBookingByAccoutPartner(email);     
        }
    }
}
