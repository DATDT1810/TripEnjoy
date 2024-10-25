using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Rate;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class RateRepository : IRateRepository
    {
        private readonly ApplicationDbContext _context;

        public RateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rate>> GetAllRateAsync()
        {
            return await _context.Rates.ToListAsync();
        }

        public async Task<Rate> GetRateByIdAsync(int rateId)
        {
            var rate = await _context.Rates.FindAsync(rateId);
            if (rate == null)
            {
                throw new ArgumentException();
            }
            return rate;
        }

        public async Task<Rate> CreateRateAsync(Rate rate)
        {
            await _context.Rates.AddAsync(rate);
            await _context.SaveChangesAsync();
            return rate;
        }

        public Task<Rate> DeteleRateAsync(int rateId)
        {
            throw new NotImplementedException();
        }

        public Task<Rate> UpdateRateAsync(Rate rate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Rate>> GetRatesForRoomAsync(int roomId)
        {
            return await _context.Rates
        .Where(r => r.RoomId == roomId)
        .Include(r => r.Account)  
        .ToListAsync();
        }
    }
}
