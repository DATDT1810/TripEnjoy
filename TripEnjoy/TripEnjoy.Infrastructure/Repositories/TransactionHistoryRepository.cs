using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface.TransactionHistories;
using TripEnjoy.Domain.Models;
using TripEnjoy.Infrastructure.Entities;

namespace TripEnjoy.Infrastructure.Repositories
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransactionHistoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateTransaction(TransactionDTO transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }
            var transactionHistory = _mapper.Map<TransactionHistory>(transaction);
            transactionHistory.TransactionType = transaction.IsCredit ?
     $"+{transaction.Amount}đ" :
     $"-{transaction.Amount}đ";
            await _context.TransactionHistories.AddAsync(transactionHistory);
            await _context.SaveChangesAsync();

        }
    }
}
