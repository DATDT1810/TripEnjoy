using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TripEnjoy.Infrastructure.Helper
{
    public class JwtHelper
    {
        public static bool IsTokenExpired(string token)
        {
            try {
                var payload =  JWT.Payload(token);

                var tokenDate = JsonSerializer.Deserialize<JwtPayload>(payload);

                if(tokenDate!= null)
                {
                    var expirationTime = DateTimeOffset.FromUnixTimeSeconds(tokenDate.exp).DateTime;
                    return expirationTime < DateTime.UtcNow;
                }
            }
            catch(Exception ex) {

                Console.WriteLine(ex.Message);
            }
            return true;
        }   
    }

    public class JwtPayload
    {
        public long exp { get; set; } 
    }
}

