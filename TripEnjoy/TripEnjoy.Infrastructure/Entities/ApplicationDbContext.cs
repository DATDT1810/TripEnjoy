using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Infrastructure.Entities
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<HotelQuestions> HotelQuestions { get; set; }
        public DbSet<ImageHotel> ImageHotels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherUser> VoucherUsers { get; set; }


        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<VoucherUser>()
           .HasKey(vu => new { vu.VoucherId, vu.AccountId }); // Thiết lập composite key

            modelBuilder.Entity<VoucherUser>()
                .HasOne(vu => vu.Account)
                .WithMany() // Nếu Account có nhiều VoucherUser
                .HasForeignKey(vu => vu.AccountId); // Thiết lập AccountId là khóa ngoại

            modelBuilder.Entity<VoucherUser>()
                .HasOne(vu => vu.Voucher)
                .WithMany() // Nếu Voucher có nhiều VoucherUser
                .HasForeignKey(vu => vu.VoucherId); // Thiết lập VoucherId là khóa ngoại

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Account1) // Mối quan hệ với Account1
                .WithMany() // Nếu không có mối quan hệ ngược, để trống
                .HasForeignKey(c => c.AccountId1)
                .OnDelete(DeleteBehavior.Restrict); // Không cho phép xóa tự động

            modelBuilder.Entity<Conversation>()
                .HasOne(c => c.Account2) // Mối quan hệ với Account2
                .WithMany() // Nếu không có mối quan hệ ngược, để trống
                .HasForeignKey(c => c.AccountId2)
                .OnDelete(DeleteBehavior.Restrict); // Không cho phép xóa tự động

            modelBuilder.Entity<Booking>()
           .HasOne(b => b.Room)                     // Mỗi Booking có một Room
           .WithMany()                              // Room không cần có danh sách Booking
           .HasForeignKey(b => b.RoomId)            // RoomId là khóa ngoại
           .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
             .HasOne(c => c.Account)                 // Mỗi Comment có một Account
             .WithMany()                              // Account không cần có danh sách Comment
             .HasForeignKey(c => c.AccountId)        // AccountId là khóa ngoại
              .OnDelete(DeleteBehavior.NoAction);      // Không cho phép xóa tự động

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Room)                     // Mỗi Comment có một Room
                .WithMany()                              // Room không cần có danh sách Comment
                .HasForeignKey(c => c.RoomId)            // RoomId là khóa ngoại
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rate>()
            .HasKey(r => r.RateId); // Thiết lập khóa chính cho Rate

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Account) // Mỗi Rate có một Account
                .WithMany() // Nếu Account có nhiều Rate
                .HasForeignKey(r => r.AccountId) // AccountId là khóa ngoại
                .OnDelete(DeleteBehavior.NoAction); // Xóa tự động

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Room) // Mỗi Rate có một Room
                .WithMany() // Nếu Room không cần có danh sách Rate
                .HasForeignKey(r => r.RoomId) // RoomId là khóa ngoại
                .OnDelete(DeleteBehavior.NoAction); // Xóa tự động
        }
    }
}
