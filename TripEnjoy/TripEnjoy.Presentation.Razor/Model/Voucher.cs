using System.ComponentModel.DataAnnotations;

namespace TripEnjoy.Presentation.Razor.Model
{
    public class Voucher
    {
        [Key]
        [Required]
        public int VoucherId { get; set; }
        [Required]
        public string VoucherName { get; set; }
        [Required]
        [StringLength(6)]
        public string VoucherCode { get; set; }
        [Required]
        public int VoucherDiscount { get; set; }
        [Required]
        public DateTime VoucherStartDate { get; set; }
        [Required]
        public DateTime VoucherEndDate { get; set; }
        public int VoucherQuantity { get; set; }
    }
}