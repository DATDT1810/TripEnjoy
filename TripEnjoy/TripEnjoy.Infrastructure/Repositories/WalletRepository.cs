using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Interface.Wallet;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;

        public WalletRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task CreateWalletUserAsync(int accountId)
        {
            if (accountId == 0)
            {
                throw new ArgumentNullException("accountId");
            }
            Wallet wallet = new Wallet
            {
                AccountId = accountId,
                WalletBalance = 0
            };
            await _context.Wallets.AddAsync(wallet);
            _context.SaveChanges();
        }

        public async Task<Wallet> GetWalletByAccountId(int accountId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task UpdateWallet(Wallet wallet)
        {
            var walletUser = await _context.Wallets.FirstOrDefaultAsync(a => a.AccountId == wallet.AccountId);
            if (walletUser == null)
            {
                throw new ArgumentNullException("Wallet not found");
            }
            _context.Wallets.Update(wallet);
            _context.SaveChanges();
        }
    }
}
