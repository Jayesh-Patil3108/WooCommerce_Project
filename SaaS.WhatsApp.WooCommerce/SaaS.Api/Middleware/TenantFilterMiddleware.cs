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

        //public async Task InvokeAsync(HttpContext context)
        //{
        //    if (context.User.Identity.IsAuthenticated)
        //    {
        //        var clientId = context.User.FindFirst("clientId")?.Value;
        //        if (string.IsNullOrEmpty(clientId))
        //        {
        //            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        //            await context.Response.WriteAsync("Client ID not found.");
        //            return;
        //        }
        //    }

        //    await _next(context);
        //}
        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("IsAuthenticated: {IsAuthenticated}", context.User?.Identity?.IsAuthenticated ?? false);

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var clientIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "clientId");
                var clientId = clientIdClaim?.Value;
                _logger.LogInformation("ClientId claim found: {ClientId}", clientId ?? "null");
                if (string.IsNullOrEmpty(clientId))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Client ID not found.");
                    return;
                }
                // Store clientId in HttpContext.Items for use in controllers
                context.Items["ClientId"] = int.Parse(clientId);
            }
            else
            {
                _logger.LogWarning("Request is not authenticated. Headers: {Headers}", string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}")));
            }

            await _next(context);
        }
    }
}
