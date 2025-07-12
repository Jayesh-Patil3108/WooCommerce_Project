namespace SaaS.Api.Middleware
{
    public class TenantFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantFilterMiddleware> _logger;
        public TenantFilterMiddleware(RequestDelegate next, ILogger<TenantFilterMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var clientId = context.User.FindFirst("clientId")?.Value;
                if (string.IsNullOrEmpty(clientId))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Client ID not found.");
                    return;
                }
            }

            await _next(context);
        }      
    }
}
