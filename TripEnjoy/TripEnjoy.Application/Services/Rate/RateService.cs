using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Rate;

namespace TripEnjoy.Application.Services.Rate
{
    public class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;

        public RateService(IRateRepository rateRepository)
        {
            _rateRepository = rateRepository;
        }

        public async Task<IEnumerable<Domain.Models.Rate>> GetAllRateAsync()
        {
            return await _rateRepository.GetAllRateAsync(); 
        }

        public async Task<Domain.Models.Rate> GetRateByIdAsync(int rateId)
        {
            return await _rateRepository.GetRateByIdAsync(rateId);
        }

        public async Task<Domain.Models.Rate> CreateRateAsync(Domain.Models.Rate rate)
        {
            return await _rateRepository.CreateRateAsync(rate);
        }

        public async Task<Domain.Models.Rate> DeteleRateAsync(int rateId)
        {
            return await _rateRepository.DeteleRateAsync(rateId);
        }

        public async Task<Domain.Models.Rate> UpdateRateAsync(Domain.Models.Rate rate)
        {
            return await _rateRepository.UpdateRateAsync(rate);
        }

        public async Task<IEnumerable<Domain.Models.Rate>> GetRatesForRoomAsync(int roomId)
        {
            return await _rateRepository.GetRatesForRoomAsync(roomId);
        }

        public async Task<bool> HasUserBookedRoomAsync(int roomId, int accountId)
        {
           return await _rateRepository.HasUserBookedRoomAsync(roomId, accountId);
        }
    }
}
