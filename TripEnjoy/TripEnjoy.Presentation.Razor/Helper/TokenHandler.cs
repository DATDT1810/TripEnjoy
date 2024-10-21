using System.Net.Http.Headers;
using System.Net;
using TripEnjoy.Presentation.Razor.Services;

namespace TripEnjoy.Presentation.Razor.Helper
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly TokenServices _tokenService;

        public TokenHandler(TokenServices tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Gắn token hiện tại vào mỗi request
            var accessToken = _tokenService.GetAccessToken();
            if (!JwtHelper.IsTokenExpired(accessToken))
            {
                // Nếu token đã hết hạn, lấy refresh token và làm mới token
                var refreshToken = _tokenService.GetRefreshToken();
                var tokenRefreshed = await _tokenService.RefreshToken(refreshToken);
                
                // Nếu làm mới thành công, lấy token mới
                if (tokenRefreshed != null)
                {
                    accessToken = _tokenService.GetAccessToken();
                }
                else
                {
                    // Nếu không làm mới được, bạn có thể xử lý theo cách khác, chẳng hạn như ném ngoại lệ hoặc trả về một phản hồi không hợp lệ
                    throw new InvalidOperationException("Unable to refresh token.");
                }
            }
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            var response = await base.SendAsync(request, cancellationToken);

            // Nếu token hết hạn (thường là 401 Unauthorized), thực hiện làm mới token
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshToken = _tokenService.GetRefreshToken();
                var tokenRefreshed = await _tokenService.RefreshToken(refreshToken);
                if (tokenRefreshed != null)
                {
                    // Gắn lại token mới vào request
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenService.GetAccessToken());
                    response = await base.SendAsync(request, cancellationToken);
                }
                else
                {                    
                    throw new InvalidOperationException("Unable to refresh token.");
                }
            }

            return response;
        }
    }
}