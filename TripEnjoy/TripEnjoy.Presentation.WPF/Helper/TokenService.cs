using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TripEnjoy.Presentation.WPF.Models;

namespace TripEnjoy.Presentation.WPF.Helper
{
    public static class TokenService
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

            var json = JsonConvert.SerializeObject(tokenResponse, Formatting.Indented);
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
    }


}
