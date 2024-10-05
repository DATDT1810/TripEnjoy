namespace TripEnjoy.Presentation.Web.Middleware
{
    public class TokenValidationMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public TokenValidationMiddleWare(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Token is required");
                return;
            }
        }
    }
}
