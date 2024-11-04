using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TripEnjoy.Presentation.WPF.Models;

namespace TripEnjoy.Presentation.WPF.Helper
{
    public class TokenHelper
    {
        private static readonly string tokenFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings", "token.json");

        public static void SaveToken(TokenResponse tokenResponse)
        {
            if (tokenResponse == null)
            {
                throw new ArgumentNullException(nameof(tokenResponse));
            }

            var directory = Path.GetDirectoryName(tokenFilePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonConvert.SerializeObject(tokenResponse, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(tokenFilePath, json);
        }

        public static TokenResponse LoadToken()
        {
            if (!File.Exists(tokenFilePath))
            {
                throw new FileNotFoundException("Token file not found.", tokenFilePath);
            }

            var json = File.ReadAllText(tokenFilePath);
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(json);
            if (tokenResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize token.");
            }

            return tokenResponse;
        }

        public static void DeleteTokenFile()
        {
            if (File.Exists(tokenFilePath))
            {
                File.Delete(tokenFilePath);
            }
        }

        public static TokenPayload GetTokenPayload(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken != null)
            {
                return new TokenPayload
                {
                    Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value,
                    Role = jwtToken.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value
                };
            }

            return null;
        }
    }
}
