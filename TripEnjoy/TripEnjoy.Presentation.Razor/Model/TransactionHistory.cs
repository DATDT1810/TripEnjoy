using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.Razor.Model
{
    public class TransactionHistory
    {
        [Key]
        public int TransactionId { get; set; }
        public int WalletId { get; set; }
        public DateTime TransactionDate { get; set; }

        // số tiền thay đổi trong tài khoản
        public decimal TransactionAmount { get; set; }
        public string TransactionType { get; set; }  // loại giao dịch
        public string TransactionDescription { get; set; }  // mô tả giao dịch

        [ForeignKey("WalletId")]
        public virtual Wallet Wallet { get; set; }
    }
}
