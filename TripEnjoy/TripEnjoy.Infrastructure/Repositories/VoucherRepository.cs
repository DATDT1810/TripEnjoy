using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.Voucher;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public VoucherRepository(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<Voucher> CreateVoucherAsync(VoucherDTO voucherDTO)
        {
           if(voucherDTO == null)
            {
                throw new ArgumentNullException(nameof(voucherDTO));
            }
           var voucher = mapper.Map<Voucher>(voucherDTO);
            if(voucher == null)
            {
                throw new ArgumentNullException(nameof(voucher));
            }
            await context.Vouchers.AddAsync(voucher);
           await  context.SaveChangesAsync();
            return voucher;
        }

        public async Task<IEnumerable<Voucher>> GetVouchersAsync()
        {
           return await context.Vouchers.ToListAsync();
        } 

      public async Task<Voucher> UpdateVoucherAsync(VoucherDTO voucherDTO)
        {
            if (voucherDTO == null)
            {
                throw new ArgumentNullException(nameof(voucherDTO));
            }
            var voucher = mapper.Map<Voucher>(voucherDTO);
            if (voucher == null)
            {
                throw new ArgumentNullException(nameof(voucher));
            }
            context.Vouchers.Update(voucher);
            await context.SaveChangesAsync();
            return voucher;
        }
        public async Task<Voucher> GetVoucherByIdAsync(int voucherId)
        {
            var voucher = await context.Vouchers.FirstOrDefaultAsync(v => v.VoucherId == voucherId);
            if(voucher == null)
            {
                throw new ArgumentNullException(nameof(voucher));
            }
            return voucher;
        }
    }
}
