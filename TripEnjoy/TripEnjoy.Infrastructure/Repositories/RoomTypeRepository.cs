using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.RoomType;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    { 
        private readonly ApplicationDbContext _context;

        public RoomTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RoomType>> GetAllRoomTypeAsync()
        {
            return await _context.RoomTypes.ToListAsync();
        }

        public async Task<RoomType> GetRoomTypeByIdAsync(int roomTypeId)
        {
            var roomType = await _context.RoomTypes.FindAsync(roomTypeId);
            if(roomType == null) {
                throw new KeyNotFoundException($"RoomType with ID {roomTypeId} not found.");
            }
            return roomType;
        }

        public async Task<RoomType> CreateRoomTypeAsync(RoomType roomType)
        {
            await _context.RoomTypes.AddAsync(roomType);
            await _context.SaveChangesAsync();
            return roomType;
        }

        public async Task<RoomType> DeleteRoomTypeAsync(int roomTypeId)
        {
            var roomType = await _context.RoomTypes.FindAsync(roomTypeId);
            if (roomType == null)
            {
                throw new KeyNotFoundException($"RoomType with ID {roomTypeId} not found.");
            }
            _context.RoomTypes.Remove(roomType);
            await _context.SaveChangesAsync();
            return roomType;

        }

        public async Task<RoomType> UpdateRoomTypeAsync(int roomTypeId, RoomType roomType)
        {
            var existingRoomType = await _context.RoomTypes.FindAsync(roomTypeId);
            if (existingRoomType == null)
            {
                throw new KeyNotFoundException($"RoomType with ID {roomTypeId} not found.");
            }
            existingRoomType.RoomTypeName = roomType.RoomTypeName;
            existingRoomType.RoomTypeStatus = roomType.RoomTypeStatus;
            _context.RoomTypes.Update(existingRoomType);
            await _context.SaveChangesAsync();
            return existingRoomType;
        }
    }
}
