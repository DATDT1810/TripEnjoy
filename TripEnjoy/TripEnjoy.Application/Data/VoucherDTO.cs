using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class VoucherDTO
    {
        public int VoucherId { get; set; }
        public string VoucherName { get; set; }
        public string VoucherDescription { get; set; }
        public string VoucherImage { get; set; }
        public int VoucherDiscount { get; set; }
        public int VoucherPoint { get; set; }
        public DateTime VoucherStartDate { get; set; }
        public DateTime VoucherEndDate { get; set; }
    
    }
}
