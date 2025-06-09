using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Services;

namespace TaskManager.Api.Middleware
{
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AuthService authService)
        {
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && !path.StartsWith("/api/auth/login") && !path.StartsWith("/api/auth/signup"))
            {
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var sessionIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "sessionid");
                    if (sessionIdClaim != null && Guid.TryParse(sessionIdClaim.Value, out var sessionId))
                    {
                        var valid = await authService.IsSessionValidAsync(sessionId);
                        if (!valid)
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Session expired or invalid.");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Session required.");
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Authentication required.");
                    return;
                }
            }
            await _next(context);
        }
    }
}
