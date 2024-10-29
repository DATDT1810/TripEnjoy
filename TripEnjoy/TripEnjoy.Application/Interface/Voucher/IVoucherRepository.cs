using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;

namespace TripEnjoy.Application.Interface.Voucher
{
    public interface IVoucherRepository
    {
        Task<Domain.Models.Voucher> CreateVoucherAsync(VoucherDTO voucherDTO);
        Task<IEnumerable<Domain.Models.Voucher>> GetVouchersAsync();
        Task<Domain.Models.Voucher> UpdateVoucherAsync(VoucherDTO voucherDTO);
        Task<Domain.Models.Voucher> GetVoucherByIdAsync(int voucherId);
    }
}
