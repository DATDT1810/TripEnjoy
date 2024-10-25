
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int AccountId { get; set; }  // account_id là khóa chính tự tăng

     
        public string AccountUsername { get; set; }  // account_username

        public string AccountPassword { get; set; }  // account_password

        public int AccountRole { get; set; }  // account_role

        public bool AccountIsDeleted { get; set; }  // account_isDeleted

        public int? WalletID { get; set; }  // account_balance

        public bool AccountUpLevel { get; set; }  // account_upLevel

        public string AccountFullname { get; set; }  // account_fullname

        public string AccountPhone { get; set; }  // account_phone

        public string AccountEmail { get; set; }  // account_email

        public string AccountAddress { get; set; }  // account_address

        public string AccountGender { get; set; }  // account_gender

        public DateTime AccountDateOfBirth { get; set; }  // account_dateOfbirth

        public string AccountImage { get; set; }  // account_image

        public string UserId { get; set; }  // Đây là khóa ngoại trỏ đến bảng AspNetUsers

    }
}
