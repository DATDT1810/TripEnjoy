using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Booking_Room;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetBookingListAsync()
        {
            return await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.Account)
            .ToListAsync();
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            // Bắt đầu transaction để đảm bảo tính toàn vẹn dữ liệu
            //using (var transaction = await _context.Database.BeginTransactionAsync())  // Chú ý await ở đây
            //{
            // Validation tính khả dụng của phòng
            var room = await _context.Rooms.Include(r => r.Hotel)
                .FirstOrDefaultAsync(r => r.RoomId == booking.RoomId);

            if (room == null)
            {
                throw new Exception("Room not found.");
            }

            // Kiểm tra xem số lượng phòng còn đủ không
            if (room.RoomQuantity < booking.RoomQuantity)
            {
                throw new Exception($"Not enough rooms available. Only {room.RoomQuantity} rooms left.");
            }

            // Xác định chủ khách sạn từ Room -> Hotel -> Account
            var hotelOwner = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == room.Hotel.AccountId);
            if (hotelOwner == null)
            {
                throw new Exception("Hotel owner not found.");
            }

            // Trang thái booking
            booking.BookingStatus = "Pending";

            // Tính tổng giá trị dựa trên số lượng phòng và giá phòng
            decimal totalPrice = booking.RoomQuantity * room.RoomPrice;

            booking.BookingTotalPrice = totalPrice;

            // Giảm số lượng phòng còn lại trong khách sạn
            room.RoomQuantity -= booking.RoomQuantity;
            _context.Update(room);

            booking.VoucherId = 1;

            var valueDiscount = _context.Vouchers.FirstOrDefault(x => x.VoucherId == booking.VoucherId);
            if (valueDiscount != null)
            {
                booking.BookingTotalPrice -= booking.BookingTotalPrice * valueDiscount.VoucherDiscount / 100;
            }

            // Lưu thông tin booking
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();  // Lưu thông tin booking trước

            // Tính toán số tiền chia cho chủ khách sạn và admin
            //decimal ownerShare = totalPrice * 0.70M;  // 70% cho chủ khách sạn
            //decimal adminShare = totalPrice * 0.30M;  // 30% cho admin

            // Tìm tài khoản admin
            //var adminAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == "f0bc4674-df5e-43e3-97a9-a280be3400de");
            //if (adminAccount == null)
            //{
            //    throw new Exception("Admin account not found.");
            //}

            // Cập nhật số dư tài khoản của chủ khách sạn và admin
            //hotelOwner.AccountBalance += ownerShare;
            //adminAccount.AccountBalance += adminShare;

            //_context.Accounts.Update(hotelOwner);
            //_context.Accounts.Update(adminAccount);

            //  await _context.SaveChangesAsync();

            // Commit transaction nếu tất cả đều thành công
            //await transaction.CommitAsync();
            //}
            return booking;
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.Account)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<Booking> CancelBookingAsync(int bookingId, int accId)
        {
            var booking = await _context.Bookings
                .Include(b => b.Account)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId && b.Account.AccountId == accId);
            if (booking == null)
            {
                throw new Exception("Booking not found or you do not have permission to cancel it");
            }
            // Check if the booking is already cancelled or completed
            if (booking.BookingStatus == "Cancelled" || booking.BookingStatus == "Completed")
            {
                throw new Exception("Booking cannot be cancelled as it is already cancelled or completed.");
            }
            //
            if (DateTime.UtcNow > booking.CheckinDate.AddHours(-1))
            {
                throw new Exception("Booking cannot be cancelled as it is within 1 hour of check-in.");
            }
            // Proceed with cancellation
            booking.BookingStatus = "Cancelled";
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
}
