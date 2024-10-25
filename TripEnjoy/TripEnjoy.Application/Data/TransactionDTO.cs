using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class TransactionDTO
    {
        public int WalletId { get; set; }
        public decimal Amount { get; set; } // Số tiền giao dịch
        public bool IsCredit { get; set; } // true: cộng, false: trừ

        public string? Description { get; set; } // Mô tả giao dịch
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
