using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class UserProfile
    {
        public int AccountId { get; set; }
        public string AccountEmail { get; set; }
        public string AccountFullname { get; set; }
        public string AccountPhone { get; set; }
        public string AccountAddress { get; set; }
        public string AccountGender { get; set; }
        public DateTime AccountDateOfBirth { get; set; }
        public string AccountImage { get; set; }
    }
}
