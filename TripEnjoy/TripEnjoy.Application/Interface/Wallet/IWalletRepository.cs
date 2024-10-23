using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Domain.Models;

namespace TripEnjoy.Application.Interface.Wallet
{
    public interface IWalletRepository
    {
        public Task CreateWalletUserAsync(int accountId);
        Task<Domain.Models.Wallet> GetWalletByAccountId(int accountId);
        Task UpdateWallet(Domain.Models.Wallet wallet);
    }
}
