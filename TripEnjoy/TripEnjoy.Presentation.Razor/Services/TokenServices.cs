using Newtonsoft.Json;
using System.Text;
using TripEnjoy.Presentation.Razor.ViewModels;

namespace TripEnjoy.Presentation.Razor.Services
{
    public class TokenServices
    {
        private readonly IHttpClientFactory httpClientFactory;

        public TokenServices(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<TokenResponse> RefreshToken(string refreshToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7126/api/Account/RefreshToken");
            request.Content = new StringContent(JsonConvert.SerializeObject(new { refreshToken }), Encoding.UTF8, "application/json");
            var client = httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromMinutes(2);
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(token);
                if (tokenResponse != null)
                {
                    return tokenResponse; // Trả về access token mới
                }
            }

            throw new Exception("Failed to refresh token");
        }
    }
}
