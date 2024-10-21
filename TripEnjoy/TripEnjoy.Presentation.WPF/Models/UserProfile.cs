using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Models
{
	public class UserProfile
	{
        public int accountId { get; set; }
        public string accountEmail { get; set; }
        public string accountFullname { get; set; }
        public string accountPhone { get; set; }
        public string accountAddress { get; set; }
        public string accountGender { get; set; }
        public DateTime accountDateOfBirth { get; set; }
        public string accountImage { get; set; }

		public override string? ToString()
		{
            return "account id"+ this.accountId;
		}
	}
}
