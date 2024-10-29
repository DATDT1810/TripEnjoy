using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.Razor.Model
{
    public class VoucherUser
    {
        [Key]
        [Column(Order = 0)]
        public int VoucherId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int? AccountId { get; set; }

        public string VoucherStatus { get; set; }

        // Điều này có thể là một điều hướng nếu bạn có bảng Account
        [ForeignKey("AccountId")]
        public virtual  Account Account { get; set; }
        [ForeignKey("VoucherId")]
        public virtual Voucher Voucher { get; set; }
    }
}
