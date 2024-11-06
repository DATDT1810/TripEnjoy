using System.ComponentModel.DataAnnotations;

namespace TripEnjoy.Presentation.Razor.ViewModels
{
    public class VoucherVM
    {

        public int VoucherId { get; set; }
        public string VoucherName { get; set; }
        public string VoucherCode { get; set; }
        public int VoucherDiscount { get; set; }
        public DateTime VoucherStartDate { get; set; }
        public DateTime VoucherEndDate { get; set; }
        public int VoucherQuantity { get; set; }
    }
}
