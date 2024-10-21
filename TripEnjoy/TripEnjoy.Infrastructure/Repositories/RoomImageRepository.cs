using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.RoomImage;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class RoomImageRepository : IRoomImageRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Domain.Models.RoomImage>> GetAllRoomImagesAsync()
        {
            return await _context.RoomImages.ToListAsync();
        }

        public async Task<Domain.Models.RoomImage> GetRoomImageByIdAsync(int roomImgId)
        {
            var roomImage = await _context.RoomImages.FindAsync(roomImgId);
            if (roomImage == null)
            {
                throw new KeyNotFoundException($"RoomImage with ID {roomImgId} not found.");
            }
            return roomImage;
        }

        public async Task<IEnumerable<RoomImage>> GetRoomImagesByRoomIdAsync(int roomId)
        {
            return await _context.RoomImages.Where(r => r.RoomId == roomId).ToListAsync();
        }
    }
}
