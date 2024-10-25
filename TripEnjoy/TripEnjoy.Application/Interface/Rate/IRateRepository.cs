using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.Rate
{
    public interface IRateRepository
    {
        Task<IEnumerable<Domain.Models.Rate>> GetAllRateAsync();
        Task<Domain.Models.Rate> GetRateByIdAsync(int rateId);
        Task<Domain.Models.Rate> CreateRateAsync(Domain.Models.Rate rate);
        Task<Domain.Models.Rate> UpdateRateAsync(Domain.Models.Rate rate);
        Task<Domain.Models.Rate> DeteleRateAsync(int rateId);
        Task<IEnumerable<Domain.Models.Rate>> GetRatesForRoomAsync(int roomId);

    }
}
