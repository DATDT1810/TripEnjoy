using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Application.Data
{
    public class PasswordRequest
    {
        public string email { get; set; }
        public string password { get; set; }
        public string code { get; set; }

    }
}
