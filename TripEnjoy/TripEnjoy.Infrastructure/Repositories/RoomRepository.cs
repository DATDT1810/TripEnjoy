using Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Room;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _context;

        public RoomRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get all room
        public async Task<IEnumerable<Room>> GetAllRoomAsync()
        {
            return await _context.Rooms.ToListAsync();
        }
        // Get room by ID
        public async Task<Room> GetRoomDetailByIdAsync(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");
            }
            return room;
        }
        // Get room by Hotel
        public async Task<IEnumerable<Room>> GetRoomsByHotelIdAsync(int hotelId)
        {
            return await _context.Rooms.Where(r => r.HotelId == hotelId).ToListAsync();
        }
        // Create a new room
        public async Task<Room> CreateRoomAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;
        }
        // Update an existing room
        public async Task<Room> UpdateRoomAsync(int roomId, Room room)
        {
            var existingRoom = await _context.Rooms.FindAsync(roomId);
            if (existingRoom == null)
            {
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");
            }
            //existingRoom.HotelId = room.HotelId;
            existingRoom.RoomTitle = room.RoomTitle;
            //existingRoom.RoomTypeId = room.RoomTypeId;
            existingRoom.RoomQuantity = room.RoomQuantity;
            //existingRoom.RoomStatusID = room.RoomStatusID;
            existingRoom.RoomPrice = room.RoomPrice;
            existingRoom.RoomDescription = room.RoomDescription;
            _context.Rooms.Update(existingRoom);
            await _context.SaveChangesAsync();

            return existingRoom;
        }
        //Delete a room by Id
        public async Task<Room> DeleteRoomAsync(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {roomId} not found.");
            }
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<IEnumerable<Room>> GetRelatedRoomsByRoomTypeIdAsync(int roomTypeId, int hotelId)
        {
            return await _context.Rooms
                         .Where(r => r.RoomTypeId == roomTypeId && r.HotelId == hotelId && r.RoomStatus.RoomStatusName == "Available")
                         .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsPartner(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            var room = _context.Rooms.Include(p => p.Hotel).ThenInclude(p => p.Account).Where(p => p.Hotel.Account.AccountEmail == email).ToList();
            return room;
        }
    }
}
