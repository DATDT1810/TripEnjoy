using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Models
{
    public class TokenRefresh
    {
        public string? RefreshToken { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
