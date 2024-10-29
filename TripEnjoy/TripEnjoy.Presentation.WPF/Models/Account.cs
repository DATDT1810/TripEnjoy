using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }  // account_id là khóa chính tự tăng

        [Required]
        [MaxLength(50)]
        public string AccountUsername { get; set; }  // account_username

        [Required]
        [MaxLength(50)]
        public string AccountPassword { get; set; }  // account_password

        public int AccountRole { get; set; }  // account_role

        public bool AccountIsDeleted { get; set; }  // account_isDeleted

        public int? WalletID { get; set; }  // account_balance

        public bool AccountUpLevel { get; set; }  // account_upLevel

        [MaxLength(100)]
        public string AccountFullname { get; set; }  // account_fullname

        [MaxLength(12)]
        public string AccountPhone { get; set; }  // account_phone

        [MaxLength(255)]
        public string AccountEmail { get; set; }  // account_email

        [MaxLength(255)]
        public string AccountAddress { get; set; }  // account_address

        [MaxLength(50)]
        public string AccountGender { get; set; }  // account_gender

        public DateTime AccountDateOfBirth { get; set; }  // account_dateOfbirth

        [MaxLength(255)]
        public string AccountImage { get; set; }  // account_image
   
    }
}
