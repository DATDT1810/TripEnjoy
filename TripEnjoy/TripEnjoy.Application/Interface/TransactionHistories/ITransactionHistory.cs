using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Interface.TransactionHistories
{
    public interface ITransactionHistory
    {
        Task CreateTransaction(int walletId);
    }
}
