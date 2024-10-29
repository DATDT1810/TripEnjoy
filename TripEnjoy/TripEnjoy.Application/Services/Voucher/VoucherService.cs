using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.Voucher;

namespace TripEnjoy.Application.Services.Voucher
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository voucherRepositoty;

        public VoucherService(IVoucherRepository voucherRepositoty)
        {
            this.voucherRepositoty = voucherRepositoty;
        }
        public Task<Domain.Models.Voucher> CreateVoucherAsync(VoucherDTO voucherDTO)
        {
            return voucherRepositoty.CreateVoucherAsync(voucherDTO);
        }

        public Task<IEnumerable<Domain.Models.Voucher>> GetVouchersAsync()
        {
           return voucherRepositoty.GetVouchersAsync();
        }

        public Task<Domain.Models.Voucher> UpdateVoucherAsync(VoucherDTO voucherDTO)
        {
            return voucherRepositoty.UpdateVoucherAsync(voucherDTO);
        }
    }
}
