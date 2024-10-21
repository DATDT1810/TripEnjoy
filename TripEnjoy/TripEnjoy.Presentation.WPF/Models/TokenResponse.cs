using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripEnjoy.Presentation.WPF.Models
{
	public class TokenResponse
	{
		public string accessToken { get; set; }
		public string refreshToken { get; set; }
        public TokenResponse()
        {
            
        }

		public TokenResponse(string accessToken, string refreshToken)
		{
			this.accessToken = accessToken;
			this.refreshToken = refreshToken;
		}
	}
}
