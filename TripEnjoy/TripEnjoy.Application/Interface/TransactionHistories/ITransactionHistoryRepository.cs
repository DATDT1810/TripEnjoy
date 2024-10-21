using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;

namespace TripEnjoy.Application.Interface.TransactionHistories
{
    public interface ITransactionHistoryRepository
    {
        Task CreateTransaction(TransactionDTO transaction);
    }
}
