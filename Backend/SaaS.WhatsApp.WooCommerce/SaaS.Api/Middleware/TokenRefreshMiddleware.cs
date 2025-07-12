//using SaaS.Application.IServices.Auth;

//namespace SaaS.Api.Middleware
//{
//    public class TokenRefreshMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public TokenRefreshMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            var authService = context.RequestServices.GetRequiredService<IAuthService>();
//            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

//            if (authHeader != null && authHeader.StartsWith("Bearer "))
//            {
//                var token = authHeader.Substring("Bearer ".Length).Trim();
//                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
//                var jwtToken = handler.ReadJwtToken(token);

//                var expiresAt = jwtToken.ValidTo.ToUniversalTime();
//                var now = DateTime.UtcNow;
//                var timeRemaining = (expiresAt - now).TotalMinutes;

//                // Check if token is about to expire (less than 2 minutes remaining, i.e., 13 minutes for 15-minute token)
//                if (timeRemaining <= 2 && timeRemaining > 0)
//                {
//                    var refreshToken = context.Request.Headers["X-Refresh-Token"].FirstOrDefault(); // Custom header for refresh token
//                    if (!string.IsNullOrEmpty(refreshToken))
//                    {
//                        try
//                        {
//                            var newAuth = await authService.RefreshTokenAsync(refreshToken);
//                            context.Response.Headers["Authorization"] = newAuth.Token; // Update response with new token
//                            context.Response.Headers["X-Refresh-Token"] = newAuth.RefreshToken; // Update refresh token
//                            context.Response.Headers["X-Expires-At"] = newAuth.ExpiresAt?.ToString("o"); // ISO 8601 format
//                        }
//                        catch (Exception ex)
//                        {
//                            context.Response.StatusCode = 401;
//                            await context.Response.WriteAsync($"Token refresh failed: {ex.Message}");
//                            return;
//                        }
//                    }
//                }
//            }

//            await _next(context);
//        }
//    }

//    // Extension method to add middleware
//    public static class TokenRefreshMiddlewareExtensions
//    {
//        public static IApplicationBuilder UseTokenRefreshMiddleware(this IApplicationBuilder builder)
//        {
//            return builder.UseMiddleware<TokenRefreshMiddleware>();
//        }
//    }
//}