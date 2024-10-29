using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;

namespace TripEnjoy.Application.Interface.Voucher
{
    public interface IVoucherService
    {
        Task<IEnumerable<TripEnjoy.Domain.Models.Voucher>> GetVouchersAsync();     
        Task<TripEnjoy.Domain.Models.Voucher> CreateVoucherAsync(VoucherDTO voucherDTO);
        Task<TripEnjoy.Domain.Models.Voucher> UpdateVoucherAsync(VoucherDTO voucherDTO);
    }
}
