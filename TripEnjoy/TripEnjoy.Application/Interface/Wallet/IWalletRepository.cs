using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.Wallet
{
    public interface IWalletRepository
    {
        public Task CreateWalletUserAsync(int accountId);
    }
}
