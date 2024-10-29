using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Models
{
	public class TokenResponse
	{
		public string? accessToken { get; set; }	
		public TokenRefresh? refreshToken { get; set; }
    }
}
